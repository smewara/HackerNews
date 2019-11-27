namespace HackerNews
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;

    /// <summary>
    /// Helper class implementing hacker news
    /// </summary>
    internal class HackerNewsHelper
    {
        //hacker news base uri
        private const string BaseUri = "https://hacker-news.firebaseio.com/";

        //rest client
        private RestClient _restClient = RestClient.Instance;

        /// <summary>
        /// Method to get up to top 500 stories
        /// </summary>
        /// <returns>http response containing top stories ids</returns>
        internal async Task<HttpResponseMessage> GetTopStoriesAsync()
        {
            string url = $"{BaseUri}v0/topstories.json";

            try
            {
                return await _restClient.Get(url);
            }
            catch(Exception ex)
            {
                throw new Exception("Error in TopStories", ex.InnerException);
            }
        }

        /// <summary>
        /// Method to get news
        /// </summary>
        /// <param name="ids">id</param>
        /// <param name="n">number of stories</param>
        /// <returns>A list of stories</returns>
        internal async Task<IList<Story>> GetNewsAsync(string[] ids, int n)
        {
            if(ids == null)
            {
                return null;
            }

            int total = ids.Length;
            int rank = 0;

            IList<Story> stories = new List<Story>();

            for (var i=0; i<n && i<total; i++)
            {
                string url = $"{BaseUri}v0/item/{ids[i]}.json";

                try
                {
                    var response = await _restClient.Get(url);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = response.Content;

                        var jsonContent = await content.ReadAsStringAsync();
                        var settings = new JsonSerializerSettings
                        {
                            ContractResolver = new CustomContractResolver()
                        };

                        var story = JsonConvert.DeserializeObject<Story>(jsonContent, settings);

                        if(!IsValidStory(story))
                        {
                            //Log here 
                            continue; //don't add this story
                        }

                        story.Rank = ++rank;

                        stories.Add(story);
                    }
                }
                catch(Exception ex)
                {
                    //Log error and continue
                    continue;
                }
            }

            return stories;
        }

        /// <summary>
        /// Validate the story
        /// </summary>
        /// <param name="story">story to be validated</param>
        /// <returns>true if valid story</returns>
        private bool IsValidStory(Story story)
        {
            if (string.IsNullOrEmpty(story.Title) || string.IsNullOrEmpty(story.Author) || story.Title.Length > 256 || story.Author.Length > 256)
            {
                return false;
            }

            if (!Uri.TryCreate(story.Uri, UriKind.Absolute, out Uri outUri)
               || (outUri.Scheme != Uri.UriSchemeHttp && outUri.Scheme != Uri.UriSchemeHttps))
            {
                return false;
            }

            if (story.Rank < 0 || story.Comments?.Length < 0 || story.Score < 0)
            {
                return false;
            }

            return true;
        }
    }
}
