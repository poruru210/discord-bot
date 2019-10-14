using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Io;
using DiscordBot.Models;

namespace DiscordBot.Services
{
    public class IndicatorService
    {
        DateTime minDate = new DateTime(2012, 1, 1);
        private Timer _timer;
        private readonly List<EconomicAnnounce> weekAnnounces = new List<EconomicAnnounce>();

        public IndicatorService()
        {
            _timer = new Timer(async state =>
            {
                var today = DateTime.Now;
                var dayOfWeek = today.AddDays(0 - (int)today.DayOfWeek);
                var results = await Get(dayOfWeek);
                lock (weekAnnounces)
                {
                    weekAnnounces.Clear();
                    weekAnnounces.AddRange(results);
                }
            }, null, TimeSpan.Zero, TimeSpan.FromHours(1));
        }

        private async Task<IEnumerable<EconomicAnnounce>> Get(DateTime time)
        {
            var requester = new DefaultHttpRequester();
            requester.Headers["User-Agent"] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36";
            var config = Configuration.Default
                .With(requester)
                .WithCulture("ja-JP")
                .WithDefaultLoader();

            var url = $"https://fx.minkabu.jp/indicators?date={time}";
            var context = BrowsingContext.New(config);
            var document = await context.OpenAsync(url);
            var tables = document.QuerySelectorAll("div.eilist table");
            return tables.SelectMany(x =>
            {
                var list = new List<EconomicAnnounce>();
                var caption = x.QuerySelector("caption").TextContent;
                var items = x.QuerySelectorAll("tr.fs-s");
                foreach (var item in items)
                {
                    var time = item.QuerySelector("td:nth-child(1) span")?.TextContent;
                    if (string.IsNullOrWhiteSpace(time))
                    {
                        continue;
                    }

                    var array = item.QuerySelector("td:nth-child(2) p").TextContent.Split('・');
                    var currency = ConvertCountryToCurrency(array[0]);
                    var name = array[1];
                    if (string.IsNullOrWhiteSpace(currency))
                    {
                        continue;
                    }

                    var economic = new EconomicAnnounce
                    {
                        Time = DateTime.ParseExact($"{caption} {time}", "yyyy年MM月dd日(ddd) HH:mm", null).ToUniversalTime(),
                        Name = name,
                        Currency = currency,
                        Importance = item.QuerySelectorAll("td:nth-child(3) img[alt*=\"Star fill\"]").Length
                    };
                    list.Add(economic);
                }
                return list;
            });
        }

        private async Task<IEnumerable<EconomicAnnounce>> Fetch(DateTime time, int count)
        {
            Console.WriteLine($"Fetch {time:yyyy-MM-dd}");

            var requester = new DefaultHttpRequester();
            requester.Headers["User-Agent"] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36";
            var config = Configuration.Default
                .With(requester)
                .WithCulture("ja-JP")
                .WithDefaultLoader();

            var url = $"https://fx.minkabu.jp/indicators?date={time:yyyy-MM-dd}&days={count}";
            var context = BrowsingContext.New(config);
            var document = await context.OpenAsync(url);
            var tables = document.QuerySelectorAll("div.eilist table");
            return tables.SelectMany(x =>
            {
                var list = new List<EconomicAnnounce>();
                var caption = x.QuerySelector("caption").TextContent;
                var items = x.QuerySelectorAll("tr.fs-s");
                foreach (var item in items)
                {
                    var time = item.QuerySelector("td:nth-child(1) span")?.TextContent;
                    if (string.IsNullOrWhiteSpace(time))
                    {
                        continue;
                    }

                    var array = item.QuerySelector("td:nth-child(2) p").TextContent.Split('・');
                    var currency = ConvertCountryToCurrency(array[0]);
                    var name = array[1];
                    if (string.IsNullOrWhiteSpace(currency))
                    {
                        continue;
                    }

                    var economic = new EconomicAnnounce
                    {
                        Time = DateTime.ParseExact($"{caption} {time}", "yyyy年MM月dd日(ddd) HH:mm", null).ToUniversalTime(),
                        Name = name,
                        Currency = currency,
                        Importance = item.QuerySelectorAll("td:nth-child(3) img[alt*=\"Star fill\"]").Length
                    };
                    list.Add(economic);
                }
                return list;
            });
        }

        public async Task<IEnumerable<EconomicAnnounce>> hoge()
        {
            var requester = new DefaultHttpRequester();
            requester.Headers["User-Agent"] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36";
            var config = Configuration.Default
                .With(requester)
                .WithCulture("ja-JP")
                .WithDefaultLoader();

            var url = "https://fx.minkabu.jp/indicators?date=2019-10-20";
            var context = BrowsingContext.New(config);
            var document = await context.OpenAsync(url);
            var tables = document.QuerySelectorAll("div.eilist table");
            return tables.SelectMany(x =>
            {
                var list = new List<EconomicAnnounce>();
                var caption = x.QuerySelector("caption").TextContent;
                var items = x.QuerySelectorAll("tr.fs-s");
                foreach (var item in items)
                {
                    var time = item.QuerySelector("td:nth-child(1) span")?.TextContent;
                    if (string.IsNullOrWhiteSpace(time))
                    {
                        continue;
                    }

                    var array = item.QuerySelector("td:nth-child(2) p").TextContent.Split('・');
                    var currency = ConvertCountryToCurrency(array[0]);
                    var name = array[1];
                    if (string.IsNullOrWhiteSpace(currency))
                    {
                        continue;
                    }

                    var economic = new EconomicAnnounce
                    {
                        Time = DateTime.ParseExact($"{caption} {time}", "yyyy年MM月dd日(ddd) HH:mm", null).ToUniversalTime(),
                        Name = name,
                        Currency = currency,
                        Importance = item.QuerySelectorAll("td:nth-child(3) img[alt*=\"Star fill\"]").Length
                    };
                    list.Add(economic);
                }
                return list;
            });
        }

        static string ConvertCountryToCurrency(string country)
        {
            switch (country)
            {
                case "アメリカ":
                    return "USD";
                case "日本":
                    return "JPN";
                case "ユーロ":
                case "ドイツ":
                    return "EUR";
                case "英国":
                    return "GBP";
                default:
                    return string.Empty;
            }

        }
    }
}
