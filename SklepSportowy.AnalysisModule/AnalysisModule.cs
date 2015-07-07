using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess.Entities;
using DataAccess.Repository.Interfaces;

namespace SklepSportowy.AnalysisModule
{
    public class AnalysisModule
    {
        private readonly IOrderDetailsRepository _orderDetailsRepository;
        private readonly IOrdersRepository _ordersRepository;
        private readonly List<int> _months = new List<int> {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12};

        public AnalysisModule(IOrderDetailsRepository orderDetailsRepository,
            IOrdersRepository ordersRepository)
        {
            _orderDetailsRepository = orderDetailsRepository;
            _ordersRepository = ordersRepository;
        }

        public Dictionary<Products, decimal> ProductsGeneratetMostIncome()
        {
            return _orderDetailsRepository
                .GetAll()
                .GroupBy(x => x.Product)
                .Select(y => new {TotalIncome = y.Sum(z => z.Quantity*z.UnitPrice), Product = y.Key})
                .OrderByDescending(o => o.TotalIncome)
                .ToDictionary(k => k.Product, p => p.TotalIncome);
        }


        public List<KeyValuePair<string, int>> GetTopProducts(int count)
        {
            var orderDetails = _orderDetailsRepository.GetAll();

            var query = (from od in orderDetails
                group od by od.Product.Name
                into g
                select new KeyValuePair<string, int>(g.Key, g.Sum(x => x.Quantity))).OrderByDescending(x => x.Value)
                .Take(count)
                .ToList();

            return query;
        }

        public List<KeyValuePair<string, int>> GetTopWorstProducts(int count)
        {
            var orderDetails = _orderDetailsRepository.GetAll();

            var query = (from od in orderDetails
                group od by od.Product.Name
                into g
                select new KeyValuePair<string, int>(g.Key, g.Sum(x => x.Quantity))).OrderBy(x => x.Value)
                .Take(count)
                .ToList();

            return query;
        }

        public List<KeyValuePair<string, int>> OrderCountByCities()
        {
            var orders = _ordersRepository.GetAll();

            var query = (from od in orders
                group od by od.User.City
                into g
                select new KeyValuePair<string, int>(g.Key, g.Count())).OrderByDescending(x => x.Value)
                .ToList();

            return query;
        }

        public Dictionary<string, int> IncomeByCity()
        {
            return
                _orderDetailsRepository.GetAll()
                    .GroupBy(x => x.Order.User.City)
                    .Select(y => new {TotalIncome = Convert.ToInt32(y.Sum(z => z.Quantity*z.UnitPrice)), City = y.Key})
                    .OrderByDescending(o => o.TotalIncome)
                    .ToDictionary(k => k.City, p => p.TotalIncome);
        }

        public Dictionary<int, decimal> GetCurrentIncome()
        {
            var date = new DateTime(DateTime.Now.Year - 1, 1, 1);

            var currentIncome = _orderDetailsRepository.GetAll()
                .Where(x => x.Order.OrderDate.Year == date.Year)
                .GroupBy(x => x.Order.OrderDate.Month)
                .Select(y => new {y.Key, TotalIncome = y.Sum(z => z.Quantity*z.UnitPrice)})
                .ToDictionary(x => x.Key, x => x.TotalIncome);


            if (currentIncome.Count < 12)
            {
                foreach (var month in _months.Where(month => !currentIncome.Keys.Contains(month)))
                    currentIncome.Add(month, Convert.ToDecimal(0.00));
            }

            return currentIncome.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
        }

        public decimal[] GetMovingAverageForNextYear(int period, Dictionary<int, decimal> currentIncome)
        {
            return MovingAverage(period, currentIncome.Values.ToArray());
        }

        public decimal[] MovingAverage(int period, decimal[] data)
        {
            var ma = new decimal[data.Length];

            decimal sum = 0;
            for (int bar = 0; bar < period; bar++)
                sum += data[bar];

            ma[period - 1] = sum/period;

            for (int bar = period; bar < data.Length; bar++)
                ma[bar] = ma[bar - 1] + (data[bar]/period) - (data[bar - period]/period);

            return ma.Select(x => Math.Round(x, 2)).ToArray();
        }
    }
}