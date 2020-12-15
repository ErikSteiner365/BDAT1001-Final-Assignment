using IdentityExample.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityExample.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TweetsController : ControllerBase
    {
        private readonly TweetsService _tweetservice;

        public TweetsController(TweetsService tweetsService)
        {
            _tweetservice = tweetsService;
        }

        [HttpGet]
        public ActionResult<List<Tweet>> Get() =>
            _tweetservice.Get();

        [HttpGet("{id:length(24)}", Name = "GetBook")]
        public ActionResult<Tweet> Get(string id)
        {
            var tweet = _tweetservice.Get(id);

            if (tweet == null)
            {
                return NotFound();
            }

            return tweet;
        }

        [HttpPost]
        public ActionResult<Tweet> Create(Tweet tweet)
        {
            _tweetservice.Create(tweet);

            return CreatedAtRoute("GetTweet", new { id = tweet.Id.ToString() }, tweet);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Tweet tweetIn)
        {
            var tweet = _tweetservice.Get(id);

            if (tweet == null)
            {
                return NotFound();
            }

            _tweetservice.Update(id, tweetIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var tweet = _tweetservice.Get(id);

            if (tweet == null)
            {
                return NotFound();
            }

            _tweetservice.Remove(tweet.Id);

            return NoContent();

        }

    }
}
