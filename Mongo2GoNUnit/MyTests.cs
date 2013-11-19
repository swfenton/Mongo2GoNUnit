using System.Linq;
using MongoDB.Driver;
using NUnit.Framework;

namespace Mongo2GoNUnit
{
    [TestFixture]
    public class MyTests : MongoIntegrationTest
    {
        private HomePageContentService _sut;
        private MongoCollection<HeroCardItem> _repository;

        [SetUp]
        public void Setup()
        {
            _repository = GetCollection<HeroCardItem>();
            _repository.Save(new HeroCardItem
            {
                Title = "Card A",
                Priority = 0
            });
            _repository.Save(new HeroCardItem
            {
                Title = "Card B",
                Priority = 1
            });

            _sut = new HomePageContentService(_repository);
        }

        [TearDown]
        public void TearDown()
        {
            Runner.Dispose();
        }

        [Test]
        public void Cards_are_created_with_top_priority()
        {
            var newCard = _sut.CreateHeroCardItem("Card C", "Some body copy.");

            Assert.That(newCard.Priority, Is.EqualTo(0));

            var cards = _sut.GetAllHeroCardItems();
            var titlesByPriority = cards.ToDictionary(i => i.Priority, i => i.Title);

            Assert.That(titlesByPriority[0], Is.EqualTo("Card C"));
            Assert.That(titlesByPriority[1], Is.EqualTo("Card A"));
            Assert.That(titlesByPriority[2], Is.EqualTo("Card B"));
        }
    }
}