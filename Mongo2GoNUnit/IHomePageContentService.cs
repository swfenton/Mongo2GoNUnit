using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;

namespace Mongo2GoNUnit
{
    public interface IHomePageContentService
    {
        HeroCardItem GetHeroCardItem(string id);
        void UpdateHeroCardItem(string id, string title, string body);
        HeroCardItem CreateHeroCardItem(string title, string body);
        void RemoveHeroCardItem(string id);
        void SetPriorityForHeroCardItem(string id, int priority);
        IEnumerable<HeroCardItem> GetAllHeroCardItems();
    }

    public class HeroCardItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Title { get; set; }
        public string Body { get; set; }
        public int Priority { get; set; }
    }

    public class HomePageContentService : IHomePageContentService
    {
        private readonly MongoCollection<HeroCardItem> _collection;

        public HomePageContentService(MongoCollection<HeroCardItem> collection)
        {
            _collection = collection;
        }

        public HeroCardItem GetHeroCardItem(string id)
        {
            throw new NotImplementedException();
        }

        public void UpdateHeroCardItem(string id, string title, string body)
        {
            throw new NotImplementedException();
        }

        public HeroCardItem CreateHeroCardItem(string title, string body)
        {
            if (string.IsNullOrWhiteSpace(title)
                || string.IsNullOrWhiteSpace(body))
            {
                throw new ArgumentException("All fields of the Hero Card must be provided.");
            }

            var item = new HeroCardItem
            {
                Title = title,
                Body = body,
                Priority = 0,
            };

            _collection.Update(Query<HeroCardItem>.Exists(c => c.Priority), Update<HeroCardItem>.Inc(c => c.Priority, 1), UpdateFlags.Multi);
            _collection.Save(item);

            return item;
        }

        public void RemoveHeroCardItem(string id)
        {
            throw new NotImplementedException();
        }

        public void SetPriorityForHeroCardItem(string id, int priority)
        {
            if (priority < 0)
                throw new ArgumentException("Priority must be 0 or greater");

            var item = GetHeroCardItem(id);
            var items = GetAllHeroCardItems().Where(i => i.Id != id).ToList();
            items.Insert(priority, item);

            var updated = items.Select((card, index) =>
            {
                card.Priority = index;
                return card;
            });

            // could this be better done by building up a multi update statement that gets executed in one go?
            foreach (var card in updated)
            {
                _collection.Save(card);
            }
        }

        public IEnumerable<HeroCardItem> GetAllHeroCardItems()
        {
            return _collection.AsQueryable().OrderBy(i => i.Priority);
        }
    }
}
