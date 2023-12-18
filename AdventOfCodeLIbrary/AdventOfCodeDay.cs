using System;
using System.IO;
using System.Net;

namespace AdventOfCodeLibrary
{
    public abstract class AdventOfCodeDay
    {
        public int Year { get; set; }
        public int Day { get; set; }
        public string Input { get; set; }
        private CookieContainer CookieContainer;
        private string SessionToken;
        public AdventOfCodeDay(int year, int day, string? session = "")
        {
            const int FIRST_YEAR = 2015;
            DateTime now = DateTime.Now;
            int nowYear = now.Year;
            int nowDay = now.Day;

            if (year < FIRST_YEAR
                || year > nowYear
                || (year == nowYear && day > nowDay)
                || day < 1
                || day > 25)
            {
                throw new ArgumentException($"Invalid date provided: {year.ToString().PadLeft(4, '0')}/{day.ToString().PadLeft(2, '0')}");
            }
            this.Year = year;
            this.Day = day;

            this.CookieContainer = new CookieContainer();
            ;
            this.SessionToken = session ?? String.Empty;
            this.SetupCookieContainer();
            this.Input = GetInputFromServer().Trim() ?? String.Empty;
        }

        public string PartOne()
        {
            return PartOne(this.Input);
        }
        public string PartTwo()
        {
            return PartTwo(this.Input);
        }
        public int SubmitPartOne(string answer)
        {
            return this.SubmitAnswer("1", answer);
        }
        public int SubmitPartTwo(string answer)
        {
            return this.SubmitAnswer("2", answer);
        }
        public abstract string PartOne(string input);
        public abstract string PartTwo(string input);
        private int SubmitAnswer(string level, string answer)
        {
            string url = $"https://adventofcode.com/{Year}/day/{Day}/answer";
            string postData = $"level={Uri.EscapeDataString(level)}&answer={Uri.EscapeDataString((string)answer)}";
            var request = BuildWebRequest(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(postData);
            }
            var response = (HttpWebResponse)request.GetResponse();
            var responseString = ReadWebResponse(response);
            if (responseString.Contains("That's the right answer"))
            {
                return 0;
            }
            else if (responseString.Contains("too high", StringComparison.OrdinalIgnoreCase))
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }

        protected string GetInputFromServer()
        {
            string year = Year.ToString();
            string day = Day.ToString();
            string url = $"https://adventofcode.com/{year}/day/{day}/input";
            var request = BuildWebRequest(url);
            request.Method = "GET";
            var response = (HttpWebResponse)request.GetResponse();
            return ReadWebResponse(response);
        }

        public void SetSession(string sessionToken)
        {
            this.SessionToken = sessionToken;
            this.SetupCookieContainer();
        }

        private void SetupCookieContainer()
        {
            var cookie = new Cookie("session", this.SessionToken, "/", "adventofcode.com");
            this.CookieContainer.Add(cookie);
        }

        private HttpWebRequest BuildWebRequest(string url)
        {
            var request = (HttpWebRequest)(WebRequest.Create(url));
            request.CookieContainer = this.CookieContainer;
            return request;
        }

        private string ReadWebResponse(HttpWebResponse response)
        {
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                return reader.ReadToEnd();
            }
        }

    }
}
