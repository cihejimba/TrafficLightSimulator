using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace TrafficlightSimulator
{
    public class TrafficlightModel
    {
        private static TrafficlightModel _instanceHandle;

        private static readonly object Padlock = new object();

        [JsonConverter(typeof(StringEnumConverter))]
        public TrafficLightStatus TrafficLightStatus { get; set; }

        public int Ttg { get; set; }
        public int Ttr { get; set; }
        private int MinCountDown { get; }
        private int MaxCountDown { get; }

        public int RandomizedCountDown => _rnd.Next(MinCountDown, MaxCountDown);

        private readonly Random _rnd;

        public TrafficlightModel()
        {
            TrafficLightStatus = TrafficLightStatus.Red;
            Ttg = 10;
            Ttr = 10;
            MinCountDown = 8;
            MaxCountDown = 12;
            _rnd = new Random();
        }

        public static TrafficlightModel Instance
        {
            get
            {
                lock (Padlock) // Thread safety.
                {
                    return _instanceHandle ?? (_instanceHandle = new TrafficlightModel());
                }
            }
        }
    }
}
