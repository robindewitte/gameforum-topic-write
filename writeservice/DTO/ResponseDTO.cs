using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fictivusforum_writeservice.DTO
{
    public class ResponseDTO
    {
        #region fields
        #endregion
        #region constructors
        public ResponseDTO()
        {

        }

        public ResponseDTO(string topicTitle, string userName, DateTime timeOfPosting, string content)
        {
            TopicTitle = topicTitle;
            UserName = userName;
            TimeOfPosting = timeOfPosting;
            Content = content;
        }
        #endregion

        #region Properties
        public string TopicTitle { get; set; }
        public string UserName { get; set; }
        public DateTime TimeOfPosting { get; set; }
        public string Content { get; set; }
        public string TopicSubject { get; set; }
        #endregion
    }
}