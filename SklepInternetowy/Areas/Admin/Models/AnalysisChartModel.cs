using System.Collections.Generic;
using DotNet.Highcharts;

namespace SklepInternetowy.Areas.Admin.Models
{
    public class AnalysisChartModel
    {
        public List<Highcharts> Charts { get; set; }
        public Dictionary<string, int> CityIncome { get; set; } 
    }
}