using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utf8Json;
using Utf8Json.Formatters;

namespace DiscordBot.Models
{
    public class EconomicAnnounce
    {
        [JsonFormatter(typeof(DateTimeFormatter), "yyyy.MM.dd HH:mm")]
        public DateTime Time { get; set; }
        public string Name { get; set; }
        public string Currency { get; set; }
        public int Importance { get; set; }
    }
}
