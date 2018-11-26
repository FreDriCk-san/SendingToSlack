using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Example23
{
    class Program
    {
        static void Main(string[] args)
        {
            LinkWithSlack.SendException();

            Console.ReadKey();
        }
    }
}
