using fictivusforum_writeservice.DataModels;
using fictivusforum_writeservice.DTO;
using fictivusforum_writeservice.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace writeservice.Controllers
{
    [Produces("application/json")]
    [Route("api/write")]
    [ApiController]
    public class WriteController : Controller
    {

        private readonly TopicContext _topicContext;

        public WriteController(TopicContext context)
        {
            _topicContext = context;
        }

        [HttpPost]
        [Route("PostTopic")]
        public async Task<bool> PostTopic(TopicDTO topicDTO)
        {
            Topic toPost = new Topic(topicDTO.Username, topicDTO.Title, topicDTO.TimeOfPosting, topicDTO.Subject);
            if (_topicContext.Topics.Where(b => b.Title == topicDTO.Title).Count() == 0)
            {
                _topicContext.Topics.Add(toPost);
                await _topicContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        [HttpPost]
        [Route("PostResponse")]
        public async Task<bool> PostResponse(ResponseDTO responseDTO)
        {
            Response toPost = new Response(responseDTO.TopicTitle, responseDTO.UserName, responseDTO.TimeOfPosting, responseDTO.Content, responseDTO.TopicSubject);
            if(_topicContext.Responses.Where(b => b.TopicTitle == responseDTO.TopicTitle && b.UserName == responseDTO.UserName && b.TimeOfPosting == responseDTO.TimeOfPosting && b.Content == responseDTO.Content).Count() == 0)
            {
                _topicContext.Responses.Add(toPost);
                await _topicContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        [HttpPost]
        [Route("DeleteTopic")]
        public async Task<bool> DeleteTopic(TopicDTO topicDTO)
        {
            Topic toNullify = await _topicContext.Topics.Where(b => b.Title == topicDTO.Title).FirstOrDefaultAsync();
            List<Response> nullifyByTopic = await _topicContext.Responses.Where(b => b.TopicTitle == topicDTO.Title).ToListAsync();
            _topicContext.Responses.RemoveRange(nullifyByTopic);
            _topicContext.Topics.Remove(toNullify);
            await _topicContext.SaveChangesAsync();
            await _topicContext.SaveChangesAsync();
            return true;
        }

        [HttpPost]
        [Route("DeleteResponse")]
        public async Task<bool> DeleteResponse(ResponseDTO responseDTO)
        {
            Response toDelete = await _topicContext.Responses.Where(b => b.TopicTitle == responseDTO.TopicTitle && b.UserName == responseDTO.UserName &&
            b.TimeOfPosting == responseDTO.TimeOfPosting && b.Content == responseDTO.Content).FirstOrDefaultAsync();
            _topicContext.Responses.Remove(toDelete);
            await _topicContext.SaveChangesAsync();
            return true;
        }
    }
}
