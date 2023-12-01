using Newtonsoft.Json;

namespace TopArticles.Models
{
    public class ArticleData
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("num_comments")]
        public long? NumComments { get; set; }

        [JsonProperty("story_id")]
        //[JsonConverter(typeof(ParseStringConverter))]
        public long? StoryId { get; set; }

        [JsonProperty("story_title")]
        public string StoryTitle { get; set; }

        [JsonProperty("story_url")]
        public Uri StoryUrl { get; set; }

        [JsonProperty("parent_id")]
        //[JsonConverter(typeof(ParseStringConverter))]
        public long? ParentId { get; set; }

        [JsonProperty("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        public string ArticleTitle
        {
            get
            {
                return Title == null ? StoryTitle : Title;
            }
        }

    }
}
