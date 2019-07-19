using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TwitterApiTest.Models
{
    public class TweetContext : DbContext
    {
        public TweetContext(DbContextOptions<TweetContext> options) : base(options)
        {

        }

        public DbSet<Tweet> Tweets { get; set; }
    }
}
