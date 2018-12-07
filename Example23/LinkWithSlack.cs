using MessagingToSlack;
using Newtonsoft.Json;
using System;
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


        public static string SendException()
        {
            try
            {
                var send = Task.Run(async () =>
                {
                    await SendExceptionAsync();
                    return "Sending done";
                });

                return send.Result;
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Main Error: {exception}");
            }

            return null;
        }



        public static async Task SendExceptionAsync()
        {
            try
            {
               // var proxy = ProxyActivation.CreateConnection();

                var info = new Info
                {
                    channel = Resources.QwiziChannel,
                    text = ExceptionSerializer(FirstLevelException())
                };

                var infoJSON = JsonConvert.SerializeObject(info);

                var request = (HttpWebRequest)WebRequest.Create("https://slack.com/api/chat.postMessage");
                request.Method = "POST";
               // request.Proxy = proxy;
                request.ContentType = "application/json";
                request.ContentLength = Encoding.UTF8.GetBytes(infoJSON).Length;
                request.Headers.Add($"Authorization: Bearer {Resources.QwiziToken}");

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



        private static Exception FirstLevelException()
        {
            try
            {
                SecondLevelException();
            }
            catch(Exception exception)
            {
                if (null != exception.InnerException)
                {
                    return exception.InnerException;
                }
                return exception;
            }

            return null;
        }



        private static void SecondLevelException()
        {
            try
            {
                ThirdLevelException();
            }
            catch (Exception inner)
            {
                try
                {
                    var file = File.Open("UnExistingFile", FileMode.Open);
                }
                catch
                {
                    throw new FileNotFoundException("Outer exception", inner);
                }
            }
        }



        private static void ThirdLevelException()
        {
            throw new NullReferenceException();
        }



        private static string ExceptionSerializer(Exception exception)
        {
            try
            {
                var exceptionList = exception.FlattenHierarchy().ToList();
                var exceptionBuilder = new StringBuilder();

                foreach (var item in exceptionList)
                {
                    var stackTrace = new StackTrace(item, true);
                    var stackFrames = stackTrace.GetFrames();

                    var traceBuilder = new StringBuilder();

                    foreach (var frame in stackFrames)
                    {
                        traceBuilder.AppendLine($"\t\t_File Column Number:_ {frame.GetFileColumnNumber()}")
                                    .AppendLine($"\t\t_File Line Number:_ {frame.GetFileLineNumber()}")
                                    .AppendLine($"\t\t_File Name:_ {frame.GetFileName()}")
                                    .AppendLine($"\t\t_Method Name:_ {frame.GetMethod().Name}")
                                    .AppendLine("\t\t*/++++++++++++/*");
                    }

                    exceptionBuilder.AppendLine($"*Date:* {DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}")
                                    .AppendLine($"*Type:* {item.GetType().FullName}")
                                    .AppendLine($"*Message:* {item.Message}")
                                    .AppendLine($"*Source:* {item.Source}")
                                    .AppendLine($"*Help:* https://stackoverflow.com/search?q={item.GetType().FullName}")
                                    .AppendLine($"*StackTrace:* \n{traceBuilder.ToString()}")
                                    .AppendLine("_/-----------------------------/_");
                }

                return exceptionBuilder.ToString();
            }
            catch(Exception except)
            {
                Console.WriteLine($"ExceptionSerializer Error: {except.Message}");
            }

            return null;
        }
    }
}
