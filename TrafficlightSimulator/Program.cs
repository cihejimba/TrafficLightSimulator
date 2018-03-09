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

        static void Main(string[] args)
        {

            Console.WriteLine("Starting server!");

            _trafficLightModel = TrafficlightModel.Instance;

            _trafficLightModel.TrafficLightStatus = TrafficLightStatus.Red;
            _trafficLightModel.TTR = 10;
            _trafficLightModel.TTG = 10;

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
                    _trafficLightModel.TTG--;
                    if (_trafficLightModel.TTG == 0)
                    {
                        _trafficLightModel.TrafficLightStatus = TrafficLightStatus.Yellow;
                        _trafficLightModel.TTR = 10;
                    }
                        
                }
                    break;
                case TrafficLightStatus.Yellow:
                {
                    if (_trafficLightModel.TTR == 10)
                    {
                        _trafficLightModel.TrafficLightStatus = TrafficLightStatus.Green;
                    }
                    else if (_trafficLightModel.TTG == 10)
                    {
                        _trafficLightModel.TrafficLightStatus = TrafficLightStatus.Red;
                    }
                }
                    break;
                case TrafficLightStatus.Green:
                    _trafficLightModel.TTR--;
                    if (_trafficLightModel.TTR == 0)
                    {
                        _trafficLightModel.TrafficLightStatus = TrafficLightStatus.Yellow;
                        _trafficLightModel.TTG = 10;
                    }
                    break;
            }

            Console.WriteLine("TTG: {0}", _trafficLightModel.TTG);
            Console.WriteLine("TTR: {0}", _trafficLightModel.TTR);
            Console.WriteLine("Traffic light status: {0}", _trafficLightModel.TrafficLightStatus.ToString());
        }
    }
}
