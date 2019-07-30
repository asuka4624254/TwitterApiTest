using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TwitterApiTest.Models;
using Newtonsoft.Json;

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

            var existingIds = _context.Tweets.Select(x => x.id);

            foreach (var tweet in tweets)
            {
                if (existingIds.Contains(tweet.Id))
                {
                    _context.Tweets.Remove(new Tweet { id = tweet.Id });
                }
                _context.Tweets.Add(new Tweet { id = tweet.Id, keyword = keyword, name = tweet.User.ScreenName, text = tweet.Text, date = tweet.CreatedAt });
            }

            _context.SaveChanges();

            foreach (var tweet in _context.Tweets)
            {
                Slack.Post(tweet);
            }

            return await _context.Tweets.ToListAsync();
        }
    }

    public static class Slack
    {
        static string WEBHOOK_URL = "https://hooks.slack.com/services/XXX";

        public static void Post(Tweet tweet)
        {
            var webClient = new WebClient();

            var data = JsonConvert.SerializeObject(new { text = tweet.ToString() });

            webClient.Headers.Add(HttpRequestHeader.ContentType, "application/json; charset=UTF-8");
            webClient.Encoding = Encoding.UTF8;
            webClient.UploadString(WEBHOOK_URL, data);
        }
    }
}