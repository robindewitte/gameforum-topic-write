using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace fictivusforum_writeservice.DataModels
{
    public class Topic
    {
        #region properties
        [Key]
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Title { get; set; }
        public DateTime TimeOfPosting { get; set; }
        public string Subject { get; set; }

        #endregion
        #region constructors
        public Topic()
        {

        }

        public Topic(string username, string title, DateTime timeOfPosting,
            string subject)
        {
            Username = username;
            Title = title;
            TimeOfPosting = timeOfPosting;
            Subject = subject;
        }
        #endregion
    }
}
