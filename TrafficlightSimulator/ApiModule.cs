using Nancy;
using Newtonsoft.Json;
using System;

namespace TrafficlightSimulator
{
    public class ApiModule : Nancy.NancyModule
    {

        public ApiModule()
        {

            int Authenticate(RequestHeaders header)
            {

                string authHeader = header.Authorization.ToString();

                // Check if an authorization header exist. If not return an error.
                if (authHeader.Length == 0)
                    return 403;

                try
                {
                    // Remove "Basic"
                    // authHeader = authHeader.Substring(6);

                    // Base64 decode.
                    // var base64EncodedBytes = System.Convert.FromBase64String(authHeader);
                    // var clearText = System.Text.Encoding.UTF8.GetString(base64EncodedBytes).ToString();

                    // Check that user and password are correct.
                    // var userAndPswd = clearText.Split(':');
                    if (authHeader != "Sommar2018")
                        return 403;

                    return 1; // All is ok.
                }
                catch (Exception e)
                {
                    //Utility.Instance.Log.Fatal(e);
                    Console.WriteLine("Password decode fatal exception: {0}", e.ToString());
                    return 500;
                }
            }

            Get["/"] = parameters =>
            {
                var returnCode = Authenticate(Request.Headers);

                if (returnCode != 1)
                    return returnCode;

                return "Hello world!";
            };

            Get["/trafficlightstatus"] = parameters =>
            {
                var returnCode = Authenticate(Request.Headers);

                if (returnCode != 1)
                    return returnCode;

                return JsonConvert.SerializeObject(TrafficlightModel.Instance);
            };

        }
    }
}
