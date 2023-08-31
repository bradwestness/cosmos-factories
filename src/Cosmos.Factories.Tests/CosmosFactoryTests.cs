namespace Cosmos.Factories.Tests;

using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Cosmos.Factories.Fakes;
using Microsoft.Azure.Cosmos;
using Moq;

public sealed class CosmosFactoryTests
{
    /// <summary>
    /// This method demonstrates how a cosmos container can be implemented
    /// in a testable way by mocking the injected IFeedIteratorFactory instance
    /// so that it doesn't call the IQueryable.ToFeedIterator() extension method
    /// when executed from the test context.
    /// </summary>
    [Fact]
    public async Task GetFeedIterator_AllowsMocking_DoesNotThrow()
    {
        var query = new[] { new Person("foo", "Foo", "Bar") }
            .AsQueryable()
            .OrderBy(x => x.Id);

        var mockCosmosContainerFactory = new Mock<ICosmosContainerFactory>();
        var mockCosmosContainer = new Mock<Container>();
        var mockFeedIteratorFactory = new Mock<IFeedIteratorFactory>();
        var testFeedIterator = new FakeFeedIterator<Person>(query);

        mockCosmosContainerFactory
            .Setup(x => x.GetContainer(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(mockCosmosContainer.Object);

        mockCosmosContainer
            .Setup(x => x.GetItemLinqQueryable<Person>(false, null, null, null))
            .Returns(query);

        mockFeedIteratorFactory
            .Setup(x => x.GetFeedIterator(It.IsAny<IQueryable<Person>>()))
            .Returns(testFeedIterator);

        var service = new PersonContainer(
            mockCosmosContainerFactory.Object,
            mockFeedIteratorFactory.Object);

        await foreach (var item in service.GetAllAsync())
        {
            Assert.NotNull(item);
        }
    }

    [Fact]
    public async Task GetFeedIterator_AllowsMocking_WithWherePredicate_DoesNotThrow()
    {
        var query = new[] { new Person("foo", "Foo", "Bar") }
            .AsQueryable()
            .OrderBy(x => x.Id);

        var mockCosmosContainerFactory = new Mock<ICosmosContainerFactory>();
        var mockCosmosContainer = new Mock<Container>();
        var mockFeedIteratorFactory = new Mock<IFeedIteratorFactory>();
        var testFeedIterator = new FakeFeedIterator<Person>(query);

        mockCosmosContainerFactory
            .Setup(x => x.GetContainer(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(mockCosmosContainer.Object);

        mockCosmosContainer
            .Setup(x => x.GetItemLinqQueryable<Person>(false, null, null, null))
            .Returns(query);

        mockFeedIteratorFactory
            .Setup(x => x.GetFeedIterator(It.IsAny<IQueryable<Person>>()))
            .Returns(testFeedIterator);

        var service = new PersonContainer(
            mockCosmosContainerFactory.Object,
            mockFeedIteratorFactory.Object);

        await foreach (var item in service.GetWhereAsync(p => p.Id == "foo"))
        {
            Assert.NotNull(item);
        }
    }

    /// <summary>
    /// Demonstration of a testable Cosmos container implementation.
    /// </summary>
    /// <typeparam name="T">The type of the document items in the cosmos collection.</typeparam>
    public interface ICosmosContainer<T>
    {
        IAsyncEnumerable<T> GetAllAsync(CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Demonstrates the usage of IFeedIteratorFactory to
    /// build a feed iterator form an ordered queryable
    /// in a testable way.
    /// </summary>
    public class PersonContainer : ICosmosContainer<Person>
    {
        private readonly Container container;
        private readonly IFeedIteratorFactory feedIteratorFactory;

        public PersonContainer(
            ICosmosContainerFactory containerFactory,
            IFeedIteratorFactory feedIteratorFactory)
        {
            this.container = containerFactory.GetContainer("foo", "bar");
            this.feedIteratorFactory = feedIteratorFactory;
        }

        /// <summary>
        /// Demonstrates getting a FeedIterator in a testable way,
        /// by calling the injected IFeedIterator method instead of using
        /// the IQueryable.ToFeedIterator() extension method directly.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async IAsyncEnumerable<Person> GetAllAsync(
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var queryable = this.container.GetItemLinqQueryable<Person>();

            // NOTE: here we are NOT calling the IQueryable.ToFeedIterator() extension
            // method directly, since it is impossible to mock. Instead we call the
            // IFeedIteratorFactory.GetFeedIterator() method, which is mockable.
            var feedIterator = this.feedIteratorFactory.GetFeedIterator(queryable);

            while (feedIterator.HasMoreResults)
            {
                foreach (var item in await feedIterator.ReadNextAsync(cancellationToken))
                {
                    yield return item;
                }
            }
        }

        public async IAsyncEnumerable<Person> GetWhereAsync(
            Expression<Func<Person, bool>> predicate,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var queryable = this.container.GetItemLinqQueryable<Person>()
                .Where(predicate);

            // NOTE: here we are NOT calling the IQueryable.ToFeedIterator() extension
            // method directly, since it is impossible to mock. Instead we call the
            // IFeedIteratorFactory.GetFeedIterator() method, which is mockable.
            var feedIterator = this.feedIteratorFactory.GetFeedIterator(queryable);

            while (feedIterator.HasMoreResults)
            {
                foreach (var item in await feedIterator.ReadNextAsync(cancellationToken))
                {
                    yield return item;
                }
            }
        }
    }

    /// <summary>
    /// A sample Cosmos document object type.
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="FirstName"></param>
    /// <param name="LastName"></param>
    public record Person(string Id, string FirstName, string LastName);
}