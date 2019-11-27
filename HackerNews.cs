namespace HackerNews
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;

    /// <summary>
    /// Class implementing hacker news
    /// </summary>
    class HackerNews
    {
        /// <summary>
        /// Main method
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            string line = string.Empty;

            if (args.Length == 2 && args[0] == "--posts")
            {
                line = args[1];
            }
            else
            {
                Console.WriteLine("Please enter command: hackernews--posts n where --posts how many posts to print.A positive integer <= 100.");

                return;
            }

            int n;

            if (!int.TryParse(line, out n))
            {
                Console.WriteLine("Please enter a numeric argument");

                return;
            }

            if(n<0 || n>100)
            {
                Console.WriteLine("Please enter a positive integer <=100");

                return;
            }

            var result = GetNewsAsync(n).GetAwaiter().GetResult();

            Console.WriteLine(result);
        }

        /// <summary>
        /// Get news
        /// </summary>
        /// <param name="n">number of top stories</param>
        /// <returns>json containing stories</returns>
        private static async Task<string> GetNewsAsync(int n)
        {
            var hackerNews = new HackerNewsHelper();

            var response = await hackerNews.GetTopStoriesAsync();

            if (response.IsSuccessStatusCode)
            {
                HttpContent content = response.Content;
                string jsonContent = await content.ReadAsStringAsync();

                var ids = JsonConvert.DeserializeObject<string[]>(jsonContent);

                IList<Story> result = await hackerNews.GetNewsAsync(ids, n);

                return JsonConvert.SerializeObject(result);
            }

            return null;
        }
    }
}
