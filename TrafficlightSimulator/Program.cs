using Nancy.Hosting.Self;
using System;
using System.Timers;

namespace TrafficlightSimulator
{


    public enum TrafficLightStatus
    {
        Green, Yellow, Red
    }

    class Program
    {

        private static TrafficlightModel _trafficLightModel;
        private static int _randomCountDown;

        static void Main(string[] args)
        {

            Console.WriteLine("Starting server!");

            _trafficLightModel = TrafficlightModel.Instance;

            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Interval = 1000;
            aTimer.Enabled = true;

            NancyHost host;

            // Run the webserver with all privileges no need to ask the user for permission.
            var urlRes = new Nancy.Hosting.Self.UrlReservations
            {
                CreateAutomatically = true
            };

            // Accept connections from outside localhost.
            var config = new HostConfiguration
            {
                RewriteLocalhost = true,
                UrlReservations = urlRes
            };

            try
            {
                host = new NancyHost(config, new Uri("http://localhost:5000"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            // Attempt to start the server.
            try
            {
                host.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            Console.ReadKey();
            host.Stop();

        }

        public static void OnTimedEvent(object o, EventArgs e)
        {
            _trafficLightModel = TrafficlightModel.Instance;

            switch (_trafficLightModel.TrafficLightStatus)
            {
                case TrafficLightStatus.Red:
                {
                    _trafficLightModel.Ttg--;
                    if (_trafficLightModel.Ttg == 0)
                    {
                        _randomCountDown = _trafficLightModel.RandomizedCountDown;
                        _trafficLightModel.TrafficLightStatus = TrafficLightStatus.Yellow;
                        _trafficLightModel.Ttr = _randomCountDown;
                    }  
                }
                break;
                case TrafficLightStatus.Yellow:
                {
                    if (_trafficLightModel.Ttr == _randomCountDown)
                    {
                        _trafficLightModel.TrafficLightStatus = TrafficLightStatus.Green;
                    }
                    else if (_trafficLightModel.Ttg == _randomCountDown)
                    {
                        _trafficLightModel.TrafficLightStatus = TrafficLightStatus.Red;
                    }
                }
                break;
                case TrafficLightStatus.Green:
                    _trafficLightModel.Ttr--;
                    if (_trafficLightModel.Ttr == 0)
                    {
                        _randomCountDown = _trafficLightModel.RandomizedCountDown;
                        _trafficLightModel.TrafficLightStatus = TrafficLightStatus.Yellow;
                        _trafficLightModel.Ttg = _randomCountDown;
                    }
                break;
            }

            Console.WriteLine("TTG: {0}", _trafficLightModel.Ttg);
            Console.WriteLine("TTR: {0}", _trafficLightModel.Ttr);
            Console.WriteLine("Traffic light status: {0}", _trafficLightModel.TrafficLightStatus.ToString());
        }
    }
}
