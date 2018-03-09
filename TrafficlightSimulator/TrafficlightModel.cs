using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TrafficlightSimulator
{
    public class TrafficlightModel
    {
        private static TrafficlightModel _instanceHandle;

        private static readonly object Padlock = new object();

        [JsonConverter(typeof(StringEnumConverter))]
        public TrafficLightStatus TrafficLightStatus { get; set; }

        public int TTG { get; set; }
        public int TTR { get; set; }
        public TrafficlightModel()
        {
            // Set default values at start-up.
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
