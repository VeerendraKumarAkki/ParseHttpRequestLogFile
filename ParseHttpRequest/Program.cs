using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;

namespace ParseHttpRequest
{
    public class ParseHttpRequests
    {
        public class ParseHttpRequest
        {
            public string ipAddress { get; set; }

            public string urls { get; set; }
        }

        static void Main(string[] args)
        {
            List<ParseHttpRequest> parseHttpRequest = new List<ParseHttpRequest>();
            try
            {
                //read the log file
                ReadTheLogFileAndExtractIpAddressAndURLs(parseHttpRequest);              

            }
            catch (Exception e)
            {
                Console.WriteLine("Error in extracting data {0}", e.Message);
                
            }
            DisplayUniqueIpAddresses(parseHttpRequest);
            DisplayTopThreeMostVisitedURLs(parseHttpRequest);
            DisplayTopThreeActiveIpAddresses(parseHttpRequest);
            Console.ReadLine();
        }

        private static void DisplayTopThreeActiveIpAddresses(List<ParseHttpRequest> parseHttpRequest)
        {
            if (parseHttpRequest.Count>0)
            {
                Console.WriteLine("\nTop 3 Most Active IpAddress");

                //Find top 3 Most active IP address 
                var mostActiveIpAddresses = parseHttpRequest.GroupBy(x => x.ipAddress).Select(y => new { Value = y.Key, Count = y.Count() }).OrderByDescending(x => x.Count).Take(3);
                foreach (var mostActiveIpAddress in mostActiveIpAddresses)
                {
                    Console.WriteLine("IpAddress => {0} , Active Count => {1}", mostActiveIpAddress.Value, mostActiveIpAddress.Count);
                }
            }

        }

        private static void DisplayTopThreeMostVisitedURLs(List<ParseHttpRequest> parseHttpRequest)
        {
            if (parseHttpRequest.Count > 0)
            {
                Console.WriteLine("\nTop 3 Most Visited URL");

                //Find the top 3 mosted visited URL order by count
                var mostVisitedURLs = parseHttpRequest.GroupBy(x => x.urls).Select(y => new { Value = y.Key, Count = y.Count() }).OrderByDescending(x => x.Count).Take(3);

                //Display information
                foreach (var mostVisitedUrl in mostVisitedURLs)
                {
                    Console.WriteLine("URL => {0} , Number Of Visits => {1}", mostVisitedUrl.Value, mostVisitedUrl.Count);
                }
            }
        }

        private static void DisplayUniqueIpAddresses(List<ParseHttpRequest> parseHttpRequest)
        {
            if (parseHttpRequest.Count > 0)
            {
                // count Uniques IPAddress using LINQ
                Console.WriteLine("Count of Unique IpAddress {0}", parseHttpRequest.Select(x => x.ipAddress).Distinct().Count());
            }
        }

        private static void ReadTheLogFileAndExtractIpAddressAndURLs(List<ParseHttpRequest> parseHttpRequest)
        {
            string filePath = Environment.CurrentDirectory + ConfigurationManager.AppSettings["LogFile"];

            IEnumerable<string> httpRequestsToProcess = ReadFile(filePath);

            if (httpRequestsToProcess != null)
            {
                // process the requests
                foreach (var request in httpRequestsToProcess)
                {
                    ParseHttpRequest parseRequest = new ParseHttpRequest();
                    if (request != null)
                    {
                        //extract ipAddress
                        var ipAddress = ExtractIpAddress(request);
                        parseRequest.ipAddress = ipAddress;

                        //extract URL 
                        var extractedUrl = ExtractUrl(request);
                        parseRequest.urls = extractedUrl;
                    }
                    // add the extracted data to the request list
                    parseHttpRequest.Add(parseRequest);
                }
            }
            else
            {
                Console.WriteLine("File content is empty");
            }
        }

        public static IEnumerable<string> ReadFile(string filePath)
        {
            return System.IO.File.ReadLines(filePath);
        }

        public static string ExtractUrl(string request)
        {
            if (request != null)
            {
                //get the index of GET_ sapace and extract the URL after GET_ hence add 4                
                int indexOfGetEnd = request.IndexOf("GET ") + Int32.Parse(ConfigurationManager.AppSettings["CharsAfterGet"]);
                int indexOfSecondSpaceAfterGet = request.IndexOf(" ", indexOfGetEnd);    //(' ', indexOfGetEnd+1);
                var url = request.Substring(indexOfGetEnd, indexOfSecondSpaceAfterGet - indexOfGetEnd);
                return url;
                
            }
            else return null;
        }

        public static string ExtractIpAddress(string request)
        {
            if (request != null)
            {
                int index = request.IndexOf('-');
                var ipAddress = request.Substring(0, index-1);
                return ipAddress;
            }
            else return null;
        }
    }
}
