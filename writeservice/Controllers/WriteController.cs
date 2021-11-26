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

        private readonly TopicContext _topicContext;

        public WriteController(TopicContext context)
        {
            _topicContext = context;
        }


        public async Task<bool> PostTopic(string username, string title, DateTime timeOfPosting,
            string subject)
        {
            Topic toPost = new Topic(username, title, timeOfPosting, subject);
            int test = _topicContext.Topics.Where(b => b.Title == title).Count();
            if (_topicContext.Topics.Where(b => b.Title == title).Count() == 0)
            {
                _topicContext.Topics.Add(toPost);
                await _topicContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> PostResponse(string topicTitle, string userName, DateTime timeOfPosting,
            string content, string topicSubject)
        {
            Response toPost = new Response(topicTitle, userName, timeOfPosting, content, topicSubject);
            if(_topicContext.Responses.Where(b => b.TopicTitle == topicTitle && b.UserName == userName && b.TimeOfPosting == timeOfPosting && b.Content == content).Count() == 0)
            {
                _topicContext.Responses.Add(toPost);
                await _topicContext.SaveChangesAsync();
                return true;
            }
            return false;
        }


        public async Task<bool> DeleteTopic(string title)
        {
            Topic toNullify = await _topicContext.Topics.Where(b => b.Title == title).FirstOrDefaultAsync();
            List<Response> nullifyByTopic = await _topicContext.Responses.Where(b => b.TopicTitle == title).ToListAsync();
            _topicContext.Responses.RemoveRange(nullifyByTopic);
            _topicContext.Topics.Remove(toNullify);
            await _topicContext.SaveChangesAsync();
            await _topicContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteResponse(string topicTitle, string username, DateTime dateTime, string content)
        {
            Response toDelete = await _topicContext.Responses.Where(b => b.TopicTitle == topicTitle && b.UserName == username &&
            b.TimeOfPosting == dateTime && b.Content == content).FirstOrDefaultAsync();
            _topicContext.Responses.Remove(toDelete);
            await _topicContext.SaveChangesAsync();
            return true;
        }
    }
}
