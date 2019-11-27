namespace HackerNews
{
    /// <summary>
    /// The story class
    /// </summary>
    public class Story
    {
        /// <summary>
        /// id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// title of the story
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// uri 
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// author
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// score
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// comments id
        /// </summary>
        public string Comments { get; set; }

        /// <summary>
        /// rank
        /// </summary>
        public int Rank { get; set; }

    }
}
