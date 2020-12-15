using IdentityExample.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityExample.Controllers
{
    public class TweetsService
    {
        private readonly IMongoCollection<Tweet> _tweets;

        public TweetsService(ITwitterDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _tweets = database.GetCollection<Tweet>(settings.CollectionName);
        }

        public List<Tweet> Get() =>
            _tweets.Find(tweet => true).ToList();

        public Tweet Get(string id) =>
             _tweets.Find<Tweet>(tweet => tweet.Id == id).FirstOrDefault();

        public Tweet Create(Tweet tweet)
        {
            _tweets.InsertOne(tweet);
            return tweet;
        }

        public void Update(string id, Tweet tweetIn) =>
            _tweets.ReplaceOne(tweet => tweet.Id == id, tweetIn);

        public void Remove(Tweet tweetIn) =>
            _tweets.DeleteOne(tweet => tweet.Id == tweetIn.Id);

        public void Remove(string id) =>
            _tweets.DeleteOne(tweet => tweet.Id == id);
    }
}
