using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LobbyReader.Common.Dtos
{
    public class MessageDto
    {
        [JsonPropertyName("channelId")]
        public string ChannelId { get; set; }
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("authorId")]
        public string AuthorId { get; set; }
        [JsonPropertyName("createdTimestamp")]
        public long CreatedTimestamp { get; set; }
        [JsonPropertyName("content")]
        public string Content { get; set; }
        [JsonPropertyName("mentions")]
        public MentionDto Mentions { get; set; }
    }
}
