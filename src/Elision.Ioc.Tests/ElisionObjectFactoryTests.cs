using FluentAssertions;
using NUnit.Framework;

namespace Elision.Foundation.Ioc.Tests
{
    [TestFixture]
    public class ElisionObjectFactoryTests
    {
        [Test]
        public void ResolvesTypeFromName()
        {
            var factory = new ElisionObjectFactory();
            var result = factory.GetObject(typeof(TestTypesForIocTesting.TestClass).AssemblyQualifiedName);
            result.Should().BeOfType<TestTypesForIocTesting.TestClass>().And.Should().NotBeNull();
        }
    }
}
