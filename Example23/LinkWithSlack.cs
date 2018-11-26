using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Example23
{
    class LinkWithSlack
    {
        private class Info
        {
            public string channel { get; set; }
            public string text { get; set; }
        }



        public static async Task SendExceptionAsync()
        {
            try
            {
                //var proxy = ProxyActivation.CreateConnection();

                var info = new Info
                {
                    channel = "GDP5T7PPT",
                    text = ExceptionSerializer(FirstLevelException()).ToString()
                };

                var infoJSON = JsonConvert.SerializeObject(info);

                var request = (HttpWebRequest)WebRequest.Create("https://slack.com/api/chat.postMessage");
                request.Method = "POST";
                //request.Proxy = proxy;
                request.ContentType = "application/json";
                request.ContentLength = Encoding.UTF8.GetBytes(infoJSON).Length;
                request.Headers.Add("Authorization: Bearer xoxp-465195059089-465800022834-478239920402-98de8f27c1b06d6abd3a4f280f4d25e9");

                using (var stream = new StreamWriter(request.GetRequestStream()))
                {
                    await stream.WriteAsync(infoJSON);
                    await stream.FlushAsync();
                    stream.Close();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Main Error: {exception}");
            }
        }



        public static void SendException()
        {
            try
            {
                //var proxy = ProxyActivation.CreateConnection();

                var info = new Info
                {
                    channel = "GDP5T7PPT",
                    text = ExceptionSerializer(FirstLevelException()).ToString()
                };

                var infoJSON = JsonConvert.SerializeObject(info);

                var request = (HttpWebRequest)WebRequest.Create("https://slack.com/api/chat.postMessage");
                request.Method = "POST";
                //request.Proxy = proxy;
                request.ContentType = "application/json";
                request.ContentLength = Encoding.UTF8.GetBytes(infoJSON).Length;
                request.Headers.Add("Authorization: Bearer xoxp-465195059089-465800022834-478239920402-98de8f27c1b06d6abd3a4f280f4d25e9");

                using (var stream = new StreamWriter(request.GetRequestStream()))
                {
                    stream.Write(infoJSON);
                    stream.Flush();
                    stream.Close();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Main Error: {exception}");
            }
        }



        private static Exception FirstLevelException()
        {
            try
            {
                SecondLevelException();
            }
            catch(Exception exception)
            {
                return exception;
            }

            return null;
        }



        private static void SecondLevelException()
        {
            ThirdLevelException();
        }



        private static void ThirdLevelException()
        {
            throw new NullReferenceException();
        }



        private static JObject ExceptionSerializer(Exception exception)
        {
            try
            {
                var stackTrace = new StackTrace(exception, true);
                var stackFrames = stackTrace.GetFrames();

                var jArray = new JArray();
                foreach (var frame in stackFrames)
                {
                    jArray.Add(new JObject(
                        new JProperty("_File Column Number:_", $"{frame.GetFileColumnNumber()}"),
                        new JProperty("_File Line Number:_", $"{frame.GetFileLineNumber()}"),
                        new JProperty("_File Name:_", $"{frame.GetFileName()}"),
                        new JProperty("_Method Name:_", $"{frame.GetMethod().Name}")
                        ));
                }

                var exceptionJSON = new JObject
                    (
                        new JProperty("*Date:*", $"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}"),
                        new JProperty("*Type:*", $"{exception.GetType().FullName}"),
                        new JProperty("*Message:*", $"{exception.Message}"),
                        new JProperty("*Source:*", $"{exception.Source}"),
                        new JProperty("*StackTrace:*", jArray)
                    );
                return exceptionJSON;
            }
            catch(Exception except)
            {
                Console.WriteLine($"ExceptionSerializer Error: {except.Message}");
            }

            return null;
        }
    }
}
