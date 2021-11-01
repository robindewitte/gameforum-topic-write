using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace fictivusforum_writeservice.DataModels
{
    public class Response
    {
        #region properties
        [Key]
        public Guid id { get; set; }

        public string TopicTitle { get; set; }
        public string UserName { get; set; }
        public DateTime TimeOfPosting { get; set; }
        public string Content { get; set; }
        #endregion

        #region constructors

        public Response() { }

        public Response(string topicTitle, string username, DateTime timeOfPosting,
            string content)
        {
            TopicTitle = topicTitle;
            UserName = username;
            TimeOfPosting = timeOfPosting;
            Content = content;
        }
        #endregion
    }
}
