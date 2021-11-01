using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fictivusforum_writeservice.DTO
{
    public class TopicDTO
    {
        #region fields
        #endregion

        #region constructors
        //empty constructor for JSON
        public TopicDTO()
        {

        }

        public TopicDTO(string username, string title, DateTime date, string subject)
        {
            Username = username;
            Title = title;
            TimeOfPosting = date;
            Subject = subject;
        }
        #endregion

        #region properties

        public string Username { get; set; }
        public string Title { get; set; }
        public DateTime TimeOfPosting { get; set; }

        public string Subject { get; set; }


        #endregion
    }
}
