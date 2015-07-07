using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;

namespace SklepInternetowy.Helpers
{
    public class ChartHelper
    {
        private static readonly Color[] BlueSetColor =
        {
            ColorTranslator.FromHtml("#A3DAFF"), ColorTranslator.FromHtml("#91D3FF"),
            ColorTranslator.FromHtml("#7ECBFF"), ColorTranslator.FromHtml("#6CC4FF"),
            ColorTranslator.FromHtml("#59BCFF"), ColorTranslator.FromHtml("#47B5FF"),
            ColorTranslator.FromHtml("#40A3E6"),
            ColorTranslator.FromHtml("#3991CC"),
            ColorTranslator.FromHtml("#4D9CD1"),
            ColorTranslator.FromHtml("#327FB2"),
            ColorTranslator.FromHtml("#3991CC"),
            ColorTranslator.FromHtml("#2B6D99"),
            ColorTranslator.FromHtml("#3382B8"),
            ColorTranslator.FromHtml("#2E74A3")
        };

        private static readonly Color[] GreenSetColor =
        {
            ColorTranslator.FromHtml("#9AC08C"),
            ColorTranslator.FromHtml("#85B376"),
            ColorTranslator.FromHtml("#71A75F"),
            ColorTranslator.FromHtml("#5D9A48"),
            ColorTranslator.FromHtml("#488E31"),
            ColorTranslator.FromHtml("#34811A"),
            ColorTranslator.FromHtml("#2F7417"),
            ColorTranslator.FromHtml("#2A6815"),
            ColorTranslator.FromHtml("#265E13"),
            ColorTranslator.FromHtml("#225511"),
            ColorTranslator.FromHtml("#3EAC3E"),
            ColorTranslator.FromHtml("#54B554"),
            ColorTranslator.FromHtml("#259325"),
            ColorTranslator.FromHtml("#218221")
        };

        // FakeChartName should be unique for each chart on page, and best solution is to provide name without white spaces (e.g. chart1, chartN)
        public static Highcharts PieChart(string chartTitle, List<object[]> data, Number width, Number height,
            string fakeChartName, string chartSubtitle)
        {
            //remove data with 0 values 
            if (data != null && data.Count > 0)
                data = data.Where(x => Convert.ToInt32(x[1]) != 0).ToList();

            var dataArray = data.ToArray();
            var chart = new Highcharts(fakeChartName)
                .InitChart(new Chart
                {
                    PlotBackgroundColor = null,
                    PlotBorderWidth = null,
                    PlotShadow = false,
                    Width = width,
                    Height = height
                })
                .SetTitle(new Title { Text = chartTitle })
                .SetTooltip(new Tooltip { Formatter = "function() {return Math.round(this.percentage) + ' %' ; }" })
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
                    Data = new Data(dataArray ?? new object[] { })
                })
                .SetCredits(new Credits { Enabled = false });

            if (!string.IsNullOrEmpty(chartSubtitle))
            {
                chart
                    .SetSubtitle(new Subtitle
                    {
                        Style = "color: '#ff0000'",
                        Text = chartSubtitle
                    });
            }


            return chart;
        }

        public static Highcharts PieChart3D(string chartTitle, string fakeChartName, Series[] series,
            string pieDataLabel = @"{point.percentage:.1f} %")
        {
            return new Highcharts(fakeChartName.Replace(" ", ""))
                .InitChart(new Chart
                {
                    DefaultSeriesType = ChartTypes.Pie,
                    PlotShadow = false,
                    PlotBackgroundColor = null,
                    PlotBorderWidth = null,
                    Height = 400,
                    Width = 450,
                    Options3d = new ChartOptions3d { Enabled = true, Alpha = 45 }
                })
                .SetTitle(new Title
                {
                    Style = "color:'#000000', fontSize: '14px'",
                    Text = chartTitle,
                    Align = HorizontalAligns.Center,
                    VerticalAlign = VerticalAligns.Middle,
                    Y = -20
                })
                .SetPlotOptions(new PlotOptions
                {
                    Pie =
                        new PlotOptionsPie
                        {
                            InnerSize = new PercentageOrPixel(40, true),
                            Depth = 45,
                            Colors = GreenSetColor,
                            ShowInLegend = true,
                            AllowPointSelect = true,
                            Cursor = Cursors.Pointer,
                            DataLabels = new PlotOptionsPieDataLabels
                            {
                                Enabled = true,
                                Format = pieDataLabel,
                                Distance = 5
                            }
                        }
                })
                .SetCredits(new Credits { Enabled = false })
                .SetSeries(series);
        }

        public static Highcharts HalfPieChart3D(string chartTitle, Number width, Number height, Series[] series,
            string fakeChartName, int? startAngle = -90, int? endAngle = -270,
            string pieDataLabel = @"<b>{point.name}</b>: {point.percentage:.1f} %")
        {
            return new Highcharts(fakeChartName.Replace(" ", ""))
                .InitChart(new Chart
                {
                    DefaultSeriesType = ChartTypes.Pie,
                    PlotShadow = false,
                    Height = height,
                    Width = width,
                    Options3d = new ChartOptions3d { Enabled = true, Alpha = 45 }
                })
                .SetTitle(new Title
                {
                    Style = "color:'#000000', fontSize: '14px'",
                    Text = chartTitle,
                    Align = HorizontalAligns.Center,
                    VerticalAlign = VerticalAligns.Middle,
                    Y = 50
                })
                .SetPlotOptions(new PlotOptions
                {
                    Pie =
                        new PlotOptionsPie
                        {
                            StartAngle = startAngle ?? -90,
                            EndAngle = endAngle ?? -270,
                            DataLabels =
                                new PlotOptionsPieDataLabels
                                {
                                    Enabled = true,
                                    Distance = 5,
                                    Format = pieDataLabel
                                },
                            InnerSize = new PercentageOrPixel(20, true),
                            Depth = 45,
                            Center = new[] { new PercentageOrPixel(50, true), new PercentageOrPixel(50, true) }
                        }
                })
                .SetCredits(new Credits { Enabled = false })
                .SetSeries(series);
        }

        public static Highcharts ColumnChart(string chartTitle, string yAxisDescriptions, string[] categories,
            Object[][] categoriesNumbers, Number width, Number height, string mouseHoverDescription,
            string fakeChartName, string chartSubtitle)
        {
            var chart = new Highcharts(fakeChartName)
                .InitChart(new Chart
                {
                    DefaultSeriesType = ChartTypes.Column,
                    Margin = new[] { 50, 50, 100, 80 },
                    Width = width,
                    Height = height
                })
                .SetTitle(new Title { Text = chartTitle })
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
                .SetLegend(new Legend { Enabled = false })
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
                    Data = new Data(categoriesNumbers ?? new object[] { })
                })
                .SetCredits(new Credits() { Enabled = false });

            if (!string.IsNullOrEmpty(chartSubtitle))
            {
                chart
                    .SetSubtitle(new Subtitle()
                    {
                        Style = "color: '#ff0000'",
                        Text = chartSubtitle
                    });
            }

            return chart;
        }

        public static Highcharts ColumnChart3D(string chartTitle, Number width, Number height, string[] categories,
            string fakeChartName, string yAxisDescription, Series[] series,
            string tooltipFormatter = @"function() { return ''+ this.x +': '+ this.y; }")
        {
            return new Highcharts(fakeChartName.Replace(" ", ""))
                .InitChart(new Chart
                {
                    DefaultSeriesType = ChartTypes.Column,
                    PlotShadow = false,
                    Height = height,
                    Width = width,
                    Options3d = new ChartOptions3d { Enabled = true, Alpha = 0, Beta = 0, Depth = 30 }
                })
                .SetTitle(new Title
                {
                    Style = "color:'#000000', fontSize: '14px'",
                    Text = chartTitle,
                })
                .SetXAxis(new XAxis
                {
                    Categories = categories,
                    Labels =
                        new XAxisLabels
                        {
                            Style = "background : '#ffffff'",
                            Enabled = true,
                            Align = HorizontalAligns.Right
                        },
                    GridLineWidth = 0
                })
                .SetYAxis(new YAxis
                {
                    Title =
                        new YAxisTitle
                        {
                            Text = yAxisDescription,
                            Style = "fontWeight: 'normal', color:'#000000' , fontSize: '11px',background : '#ffffff'"
                        },
                    StackLabels =
                        new YAxisStackLabels
                        {
                            Enabled = true,
                            Style = "fontWeight: 'normal', color:'#000000' , fontSize: '11px',background : '#ffffff'"
                        }
                })
                .SetTooltip(new Tooltip
                {
                    Enabled = true,
                    Shared = true,
                    Formatter = tooltipFormatter
                })
                .SetPlotOptions(new PlotOptions
                {
                    Column =
                        new PlotOptionsColumn
                        {
                            Stacking = Stackings.Normal,
                            Depth = 30,
                            PointWidth = 30,
                            ColorByPoint = true,
                            Colors = BlueSetColor
                        },
                    Series =
                        new PlotOptionsSeries
                        {
                            DataLabels = new PlotOptionsSeriesDataLabels { Enabled = false, Format = "{point.y}" }
                        }
                })
                .SetCredits(new Credits { Enabled = false })
                .SetLegend(new Legend
                {
                    Enabled = false,
                    BorderWidth = 0,
                    ItemStyle = "color:'#000000', fontSize: '11px', background : '#ffffff'"
                })
                .SetSeries(series);
        }

        public static Highcharts StackColumnChart(string chartTitle, Number width, Number height, string[] categories,
            string yLabel,
            Series[] seriesData, string fakeChartName, string chartSubtitle, string tooltipFormatter)
        {
            var chart = new Highcharts(fakeChartName.Replace(" ", ""))
                .InitChart(new Chart { DefaultSeriesType = ChartTypes.Column, Width = width, Height = height })
                .SetTitle(new Title { Text = chartTitle })
                .SetXAxis(new XAxis { Categories = categories })
                .SetYAxis(new YAxis
                {
                    Min = 0,
                    Title =
                        new YAxisTitle
                        {
                            Text = yLabel,
                            Style = "background: '#ffffff',fontSize: '13px',fontFamily: 'Verdana, sans-serif'"
                        },
                    StackLabels = new YAxisStackLabels { Enabled = true },
                })
                .SetLegend(new Legend { BorderWidth = 0 })
                .SetTooltip(new Tooltip
                {
                    Enabled = true,
                    FollowPointer = true,
                    Formatter = tooltipFormatter
                })
                .SetPlotOptions(new PlotOptions
                {
                    Column = new PlotOptionsColumn
                    {
                        Stacking = Stackings.Normal,
                        DataLabels = new PlotOptionsColumnDataLabels { Enabled = false }
                    }
                })
                .SetSeries(seriesData ?? new Series[] { })
                .SetCredits(new Credits { Enabled = false });

            if (!string.IsNullOrEmpty(chartSubtitle))
            {
                chart
                    .SetSubtitle(new Subtitle
                    {
                        Style = "color: '#ff0000'",
                        Text = chartSubtitle
                    });
            }

            return chart;
        }

        public static Highcharts LineChart(string chartTitle, Number width, Number height, string[] categories,
            Series[] series, string fakeChartName, string yAxisDescriptions, string chartSubtitle, string pointFormatter,
            string plotOptionsLineMarker)
        {
            var chart = new Highcharts(fakeChartName.Replace(" ", ""))
                .InitChart(new Chart
                {
                    Width = width,
                    Height = height,
                    DefaultSeriesType = ChartTypes.Line,
                })
                .SetTitle(new Title { Text = chartTitle })
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
                    Title = new YAxisTitle { Text = yAxisDescriptions, Style = "background: '#ffffff'" }
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
                                        @"function() { return '<span style=""font-size:10px; font-family:Calibri"">' + this.y + '</span>' ;}"
                                }
                        }
                })
                .SetTooltip(new Tooltip
                {
                    PointFormat = pointFormatter,
                    Enabled = true,
                    FollowPointer = true,
                    Shared = true,
                    HeaderFormat = "",
                    Crosshairs = new Crosshairs(true)
                })
                .SetLegend(new Legend { BorderWidth = 0 })
                .SetSeries(series ?? new Series[] { })
                .SetCredits(new Credits { Enabled = false });

            if (!string.IsNullOrEmpty(chartSubtitle))
            {
                chart
                    .SetSubtitle(new Subtitle
                    {
                        Style = "color: '#ff0000'",
                        Text = chartSubtitle
                    });
            }

            if (!string.IsNullOrEmpty(plotOptionsLineMarker))
            {
                chart.SetPlotOptions(new PlotOptions
                {
                    Line =
                        new PlotOptionsLine
                        {
                            Marker = new PlotOptionsLineMarker { Symbol = plotOptionsLineMarker, Enabled = true }
                        }
                });
            }

            return chart;
        }

        // Choose of one using series array or data
        public static Highcharts StackColumnChart3D(string chartTitle, Number width, Number height, string[] categories,
            string xAxisLTitle, string yAxisTitle, Series[] series, string headerFormat, string fakeTitleName,
            Color[] colors)
        {
            return new Highcharts(fakeTitleName.Replace(" ", ""))
                .InitChart(new Chart
                {
                    DefaultSeriesType = ChartTypes.Column,
                    PlotShadow = false,
                    Height = height,
                    Width = width,
                    Options3d = new ChartOptions3d { Enabled = true, Alpha = 0, Beta = 0, Depth = 30 }
                })
                .SetTitle(new Title { Style = "color:'#000000', fontSize: '14px'", Text = chartTitle })
                .SetXAxis(new XAxis
                {
                    Categories = categories,
                    Labels =
                        new XAxisLabels
                        {
                            Style = "background : '#ffffff'",
                            Enabled = true,
                            Align = HorizontalAligns.Right
                        },
                    Title =
                        new XAxisTitle
                        {
                            Text = xAxisLTitle,
                            Style = "fontWeight: 'normal', color:'#000000' , fontSize: '11px',background : '#ffffff'"
                        },
                    GridLineWidth = 0
                })
                .SetYAxis(new YAxis
                {
                    Title =
                        new YAxisTitle
                        {
                            Text = yAxisTitle,
                            Style = "fontWeight: 'normal', color:'#000000' , fontSize: '11px',background : '#ffffff'"
                        },
                    StackLabels =
                        new YAxisStackLabels
                        {
                            Enabled = true,
                            Style = "fontWeight: 'normal', color:'#000000' , fontSize: '11px',background : '#ffffff'"
                        }
                })
                .SetTooltip(new Tooltip
                {
                    Enabled = true,
                    Shared = true,
                    HeaderFormat = headerFormat
                })
                .SetPlotOptions(new PlotOptions
                {
                    //Column = new PlotOptionsColumn {Stacking = Stackings.Normal, Depth = 30},
                    Column =
                        new PlotOptionsColumn
                        {
                            Stacking = Stackings.Normal,
                            Depth = 30,
                            PointWidth = 30,
                            ColorByPoint = false
                        },
                    Series =
                        new PlotOptionsSeries
                        {
                            DataLabels = new PlotOptionsSeriesDataLabels { Enabled = false, Format = "{point.y}" }
                        }
                })
                .SetCredits(new Credits { Enabled = false })
                .SetLegend(new Legend
                {
                    BorderWidth = 0,
                    ItemStyle = "color:'#000000', fontSize: '11px', background : '#ffffff'"
                })
                .SetSeries(series ?? new Series[] { });
        }

        public static Highcharts GaugeChart(string chartTitle, string fakeChartName, Number width, Number height, Number yAxisMaxValue, Number yAxisPlotBandMaxValue, Series[] series)
        {
            return new Highcharts(fakeChartName.Replace(" ", ""))
                .InitChart(new Chart
                {
                    DefaultSeriesType = ChartTypes.Gauge,
                    PlotShadow = false,
                    PlotBackgroundColor = null,
                    PlotBorderWidth = null,
                    Height = height,
                    Width = width
                })
                .SetYAxis(new YAxis
                {
                    Min = 0,
                    Max = yAxisMaxValue,
                    LineWidth = 0,
                    Labels = new YAxisLabels { Y = 5 },
                    PlotBands = new[] { new YAxisPlotBands { Color = Color.Green, From = 0, To = yAxisPlotBandMaxValue } }
                })
                .SetTitle(new Title
                {
                    Style = "color:'#000000', fontSize: '14px'",
                    Text = chartTitle,
                    Align = HorizontalAligns.Center,
                    VerticalAlign = VerticalAligns.Top,
                    Y = 20
                })
                .SetPane(new Pane
                {
                    Size = new PercentageOrPixel(150, true),
                    StartAngle = -90,
                    EndAngle = 90,
                    Center = new[] { new PercentageOrPixel(50, true), new PercentageOrPixel(100, true) },
                    Background =
                        new[]
                        {
                            new BackgroundObject
                            {
                                InnerRadius = new PercentageOrPixel(60, true),
                                OuterRadius = new PercentageOrPixel(100, true)
                            }
                        }
                })
                .SetPlotOptions(new PlotOptions
                {
                    Gauge =
                        new PlotOptionsGauge
                        {
                            Pivot = new PlotOptionsGaugePivot { BackgroundColor = new BackColorOrGradient(Color.Green) },
                            DataLabels =
                                new PlotOptionsGaugeDataLabels
                                {
                                    Enabled = true,
                                    BorderWidth = 0,
                                    Style = "fontSize: '30px'",
                                    Y = -30
                                }
                        }
                })
                .SetCredits(new Credits { Enabled = false })
                .SetSeries(series);
        }
    }
}

