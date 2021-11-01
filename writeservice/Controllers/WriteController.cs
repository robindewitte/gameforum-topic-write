using fictivusforum_writeservice.DataModels;
using fictivusforum_writeservice.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace writeservice.Controllers
{
    public class WriteController : Controller
    {

        private readonly ResponseContext _responseContext;
        private readonly TopicContext _topicContext;
       public void DoWriteStuffMock()
       {
            Console.WriteLine(DateTime.Now.ToString() + DateTime.Now.Millisecond.ToString());
       }

        public async Task PostTopic(string username, string title, DateTime timeOfPosting,
            string subject)
        {
            Topic toPost = new Topic(username, title, timeOfPosting, subject);
            _topicContext.Add(toPost);
            await _topicContext.SaveChangesAsync();
        }

        public async Task PostResponse(string topicTitle, string userName, DateTime timeOfPosting,
            string content)
        {
            Response toPost = new Response(topicTitle, userName, timeOfPosting, content);
            _responseContext.Add(toPost);
            await _responseContext.SaveChangesAsync();
        }


        public async Task DeleteTopic(string title)
        {
            Topic toNullify = await _topicContext.Topics.Where(b => b.Title == title).FirstOrDefaultAsync();
            List<Response> nullifyByTopic = await _responseContext.Responses.Where(b => b.TopicTitle == title).ToListAsync();
            _responseContext.RemoveRange(nullifyByTopic);
            _topicContext.Remove(toNullify);
            await _responseContext.SaveChangesAsync();
            await _topicContext.SaveChangesAsync();
        }

        public async Task DeleteResponse(string topicTitle, string username, DateTime dateTime, string content)
        {
            Response toDelete = await _responseContext.Responses.Where(b => b.TopicTitle == topicTitle && b.UserName == username &&
            b.TimeOfPosting == dateTime && b.Content == content).FirstOrDefaultAsync();
            _responseContext.Remove(toDelete);
            await _responseContext.SaveChangesAsync();
        }
    }
}
