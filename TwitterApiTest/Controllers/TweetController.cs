using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TwitterApiTest.Models;

namespace TwitterApiTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TweetController : ControllerBase
    {
        private const string CONSUMER_KEY = "XXX";
        private const string CONSUMER_SECRET = "XXX";
        private const string ACCESS_TOKEN = "XXX";
        private const string ACCESS_SECRET = "XXX";

        private readonly TweetContext _context;

        public TweetController(TweetContext context)
        {
            _context = context;
        }

        // GET: api/tweet/[検索キーワード]
        [HttpGet("{keyword}")]
        public async Task<ActionResult<IEnumerable<Tweet>>> GetTweets(string keyword)
        {
            var tokens = CoreTweet.Tokens.Create(CONSUMER_KEY, CONSUMER_SECRET, ACCESS_TOKEN, ACCESS_SECRET);
            var tweets = await tokens.Search.TweetsAsync(count => 3, q => keyword);

            foreach (var tweet in tweets)
            {
                _context.Tweets.Add(new Tweet { id = tweet.Id, keyword = keyword, name = tweet.User.ScreenName, text = tweet.Text });
            }

            _context.SaveChanges();

            return await _context.Tweets.ToListAsync();
        }
    }
}