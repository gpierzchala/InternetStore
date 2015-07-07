using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.Mvc;
using DataAccess.Repository.Interfaces;
using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using SklepInternetowy.Areas.Admin.Models;
using SklepSportowy.AnalysisModule;

namespace SklepInternetowy.Areas.Admin.Controllers
{
    public class AnalysisController : Controller
    {
        private readonly AnalysisModule _analysisModule;

        public AnalysisController(IOrdersRepository ordersRepository, IOrderDetailsRepository orderDetailsRepository)
        {
            _analysisModule = new AnalysisModule(orderDetailsRepository, ordersRepository);
        }


        public ActionResult Index()
        {
            return View();
        }

        [OutputCache(Duration = 0, VaryByParam = "*")]
        public PartialViewResult OriginAnalysis()
        {
            var ordersByCity = _analysisModule.OrderCountByCities();
            var incomeByCity = _analysisModule.IncomeByCity();
            var totalIncome = incomeByCity.Sum(x => x.Value);
            var incomeChartData = new List<object[]>();
            var cities = ordersByCity.Select(x => x.Key).OrderBy(x => x).ToArray();
            var orderByCityData = ordersByCity.Select(x => x.Value).Cast<object>().ToArray();
            var viewModel = new AnalysisChartModel
            {
                CityIncome = new Dictionary<string, int>(),
                Charts = new List<Highcharts>()
            };
            var incomeSum = 0;

            foreach (var income in incomeByCity)
            {
                var obj = new object[2];
                obj[0] = income.Key;
                obj[1] = (int) Math.Round((double) (100*income.Value)/totalIncome);
                incomeChartData.Add(obj);
                viewModel.CityIncome.Add(income.Key, income.Value);
                incomeSum += (int) Math.Round((double) (100*income.Value)/totalIncome);
            }

            if (incomeSum < 100)
            {
                var lastItem = incomeChartData.LastOrDefault();
                lastItem[1] = Convert.ToInt32(lastItem[1]) + (100 - incomeSum);
            }

            viewModel.CityIncome.Add("Zysk całkowity", totalIncome);
            viewModel.Charts.Add(ColumnChart("Ilość dokonanych zamówień względem miejscowości",
                "Ilość dokonanych zamówień", cities,
                orderByCityData, 1300, 400, "Ilość", "chart1"));
            viewModel.Charts.Add(PieChart("Procentowy dochów wygenerowany w poszczególnych miejscowościach",
                incomeChartData,
                1300, 400, "chart2"));

            return PartialView("_AnalysisChartSection", viewModel);
        }


        [OutputCache(Duration = 0, VaryByParam = "*")]
        public PartialViewResult ProductAnalysis()
        {
            var top5products = _analysisModule.GetTopProducts(5);
            var top5worstProducts = _analysisModule.GetTopWorstProducts(5);

            var top5Data = new List<object[]>();
            var top5WorstData = new List<object[]>();
            var top5Sum = 0;

            var top5Total = top5products.Sum(x => x.Value);
            var top5WorstTotal = top5worstProducts.Sum(x => x.Value);
            var top5WorstSum = 0;

            foreach (var product in top5products)
            {
                var obj = new object[2];
                obj[0] = product.Key;
                obj[1] = (int) Math.Round((double) (100*product.Value)/top5Total);
                top5Data.Add(obj);
                top5Sum += (int) Math.Round((double) (100*product.Value)/top5Total);
            }

            if (top5Sum < 100)
            {
                var lastItem = top5Data.LastOrDefault();
                lastItem[1] = Convert.ToInt32(lastItem[1]) + (100 - top5Sum);
            }

            foreach (var product in top5worstProducts)
            {
                var obj = new object[2];
                obj[0] = product.Key;
                obj[1] = (int) Math.Round((double) (100*product.Value)/top5WorstTotal);
                top5WorstData.Add(obj);
                top5WorstSum += (int) Math.Round((double) (100*product.Value)/top5WorstTotal);
            }

            if (top5WorstSum < 100)
            {
                var lastItem = top5WorstData.LastOrDefault();
                lastItem[1] = Convert.ToInt32(lastItem[1]) + (100 - top5WorstSum);
            }

            var viewModel = new AnalysisChartModel
            {
                Charts = new List<Highcharts>()
                {
                    PieChart("Top 5 najczęściej kupowanych produktów",
                        top5Data,
                        1300, 400, "chart1"),
                    PieChart("Top 5 najgorzej sprzedających się produktów",
                        top5WorstData,
                        1300, 400, "chart2")
                }
            };


            return PartialView("_AnalysisChartSection", viewModel);
        }

        [OutputCache(Duration = 0, VaryByParam = "*")]
        public PartialViewResult PredictAnalysis()
        {
            var currentIncome = _analysisModule.GetCurrentIncome();
            var movingAverageForCurrentIncome = _analysisModule.GetMovingAverageForNextYear(3, currentIncome);

            for (int i = 1; i < 12; i++)
            {
                currentIncome[i] = Math.Round(currentIncome[i], 2);
                movingAverageForCurrentIncome[i] = Math.Round(movingAverageForCurrentIncome[i], 2);
            }

            var months =
                currentIncome.Select(x => x.Key).OrderBy(x => x).Select(x => Enum.GetName(typeof (Months), x)).ToArray();

            var series = new List<Series>
            {
                new Series
                {
                    Name = "Przychód bieżący",
                    Data = new Data(currentIncome.Select(x => x.Value).Cast<object>().ToArray())
                },
                new Series
                {
                    Name = "Przychód przewidywany",
                    Data = new Data(movingAverageForCurrentIncome.Select(x => x).Cast<object>().ToArray())
                }
            };

            var viewModel = new AnalysisChartModel
            {
                Charts = new List<Highcharts>
                {
                    LineChart("Przewidywany miesięczny przychód bazujący na bieżącym", 1300, 400, months,
                        series.ToArray(), "chart1", "Dochód")
                }
            };

            return PartialView("_AnalysisChartSection", viewModel);
        }

        public static Highcharts LineChart(string chartTitle, Number width, Number height, string[] categories,
            Series[] series, string fakeChartName, string yAxisDescriptions)
        {
            return new Highcharts(fakeChartName.Replace(" ", ""))
                .InitChart(new Chart
                {
                    Width = width,
                    Height = height,
                    DefaultSeriesType = ChartTypes.Line,
                })
                .SetTitle(new Title {Text = chartTitle})
                .SetXAxis(new XAxis
                {
                    Categories = categories,
                    Labels =
                        new XAxisLabels
                        {
                            Style = "background: '#ffffff'",
                            Enabled = true,
                            Align = HorizontalAligns.Center
                        }
                })
                .SetYAxis(new YAxis
                {
                    Title = new YAxisTitle {Text = yAxisDescriptions, Style = "background: '#ffffff'"}
                })
                .SetPlotOptions(new PlotOptions
                {
                    Series =
                        new PlotOptionsSeries
                        {
                            DataLabels =
                                new PlotOptionsSeriesDataLabels
                                {
                                    Enabled = false,
                                    Style = "background: '#ffffff'",
                                    Formatter =
                                        @"function() { return '<span style=""font-size:10px; font-family:Calibri"">' + this.y + ' zł</span>' ;}"
                                }
                        }
                })
                .SetTooltip(new Tooltip
                {
                    Formatter = @"function() { return '<span style=""font-size:10px; font-family:Calibri"">' + this.y + ' zł</span>' ;}",
                    Enabled = true,
                    FollowPointer = true,
                    Shared = true,
                    HeaderFormat = "",
                    Crosshairs = new Crosshairs(true)
                })
                .SetLegend(new Legend {BorderWidth = 0})
                .SetSeries(series ?? new Series[] {})
                .SetCredits(new Credits {Enabled = false});
        }


        public Highcharts ColumnChart(string chartTitle, string yAxisDescriptions, string[] categories,
            object[] data, Number width, Number height, string mouseHoverDescription,
            string fakeChartName)
        {
            return new Highcharts(fakeChartName)
                .InitChart(new Chart
                {
                    DefaultSeriesType = ChartTypes.Column,
                    Margin = new[] {50, 50, 100, 80},
                    Width = width,
                    Height = height
                })
                .SetTitle(new Title {Text = chartTitle})
                .SetXAxis(new XAxis
                {
                    Categories = categories,
                    Labels = new XAxisLabels
                    {
                        Rotation = -45,
                        Align = HorizontalAligns.Right,
                        Style = "background: '#ffffff',fontSize: '13px',fontFamily: 'Verdana, sans-serif'"
                    },
                })
                .SetYAxis(new YAxis
                {
                    Min = 0,
                    Title =
                        new YAxisTitle
                        {
                            Text = yAxisDescriptions,
                            Style = "background: '#ffffff',fontSize: '13px',fontFamily: 'Verdana, sans-serif'",
                        }
                })
                .SetLegend(new Legend {Enabled = false})
                .SetTooltip(new Tooltip
                {
                    Enabled = true,
                    FollowPointer = true,
                    Formatter =
                        @"function() { return '<span style=""color: '+this.series.color+'"">' + '" +
                        mouseHoverDescription + @"' + '</span> <b> '+ this.y +'</b>'; }"
                })
                .SetSeries(new Series
                {
                    Data = new Data(data),
                    PlotOptionsColumn = new PlotOptionsColumn
                    {
                        PointWidth = 60
                    }
                })
                .SetCredits(new Credits() {Enabled = false});
        }

//        public Highcharts LineChart(string chartTitle, string yAxisDescriptions, string[] categories, object[] data,
//            Number width,
//            Number height, string fakeChartName)
//        {
//            return new Highcharts(fakeChartName)
//                .InitChart(new Chart
//                {
//                    DefaultSeriesType = ChartTypes.Line,
//                    MarginRight = 130,
//                    MarginBottom = 25
//                })
//                .SetTitle(new Title
//                {
//                    Text = chartTitle,
//                    X = -20
//                })
//                .SetXAxis(new XAxis {Categories = categories})
//                .SetYAxis(new YAxis
//                {
//                    Title = new YAxisTitle {Text = yAxisDescriptions},
//                    PlotLines = new[]
//                    {
//                        new YAxisPlotLines
//                        {
//                            Value = 0,
//                            Width = 1,
//                            Color = ColorTranslator.FromHtml("#808080")
//                        }
//                    }
//                })
//                .SetTooltip(new Tooltip
//                {
//                    Formatter = @"function() {
//                                        return '<b>'+ this.series.name +'</b><br/>'+
//                                    this.x +': '+ this.y +'°C';
//                                }"
//                })
//                .SetLegend(new Legend
//                {
//                    Layout = Layouts.Vertical,
//                    Align = HorizontalAligns.Right,
//                    VerticalAlign = VerticalAligns.Top,
//                    X = -10,
//                    Y = 100,
//                    BorderWidth = 0
//                })
//                .SetSeries(new Series
//                {
//                    Data = new Data(data),
//                });
//        }

        public Highcharts PieChart(string chartTitle, List<object[]> data, Number width, Number height,
            string fakeChartName)
        {
            //remove data with 0 values 
            if (data != null && data.Count > 0)
                data = data.Where(x => Convert.ToInt32(x[1]) != 0).ToList();

            var dataArray = data.ToArray();
            return new Highcharts(fakeChartName)
                .InitChart(new Chart
                {
                    PlotBackgroundColor = null,
                    PlotBorderWidth = null,
                    PlotShadow = false,
                    Width = width,
                    Height = height
                })
                .SetTitle(new Title {Text = chartTitle})
                .SetTooltip(new Tooltip {Formatter = "function() {return Math.round(this.percentage) + ' %' ; }"})
                .SetPlotOptions(new PlotOptions
                {
                    Pie = new PlotOptionsPie
                    {
                        AllowPointSelect = true,
                        Cursor = Cursors.Pointer,
                        DataLabels = new PlotOptionsPieDataLabels
                        {
                            Enabled = true,
                            Color = ColorTranslator.FromHtml("#000000"),
                            ConnectorColor = ColorTranslator.FromHtml("#000000"),
                            Formatter = "function() { return Math.round(this.percentage) +' %'; }",
                            Style = "fontSize: '13px',fontFamily: 'Verdana, sans-serif'"
                        },
                        ShowInLegend = true
                    }
                })
                .SetSeries(new Series
                {
                    Type = ChartTypes.Pie,
                    Data = new Data(dataArray ?? new object[] {})
                })
                .SetCredits(new Credits {Enabled = false});
        }

        private enum Months
        {
            Styczeń = 1,
            Luty,
            Marzec,
            Kwiecień,
            Maj,
            Czerwiec,
            Lipiec,
            Sierpnień,
            Wrzesień,
            Październik,
            Listopad,
            Grudzień
        }
    }
}