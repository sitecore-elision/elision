using FluentAssertions;
using NUnit.Framework;

namespace Elision.Foundation.Ioc.Tests
{
    [TestFixture]
    public class OnDemandResolverTests
    {
        [Test]
        public void ResolvesInterfaceToDefaultType()
        {
            var resolver = new OnDemandResolver();
            var result = resolver.Resolve(typeof (TestTypesForIocTesting.ITestClass));
            result.Should().NotBeNull();
            result.Should().BeOfType<TestTypesForIocTesting.TestClass>();
        }

        [Test]
        public void DoesNotResolveInterfaceWithMultipleImplementations()
        {
            var resolver = new OnDemandResolver();
            var result = resolver.Resolve(typeof(TestTypesForIocTesting.ITestClassMultipleImpl));
            result.Should().BeNull();
        }

        [Test]
        public void CanResolveTypeWithoutConstructor()
        {
            var resolver = new OnDemandResolver();
            var result = resolver.Resolve(typeof(TestTypesForIocTesting.ITestClassNoCtor));
            result.Should().NotBeNull();
            result.Should().BeOfType<TestTypesForIocTesting.TestClassNoCtor>();
        }

        [Test]
        public void UsesParameterizedConstructorOverDefaultConstructor()
        {
            var resolver = new OnDemandResolver();
            var result = (TestTypesForIocTesting.ITestClassMultipleCtor)resolver.Resolve(typeof(TestTypesForIocTesting.ITestClassMultipleCtor));
            result.CtorCalled.Should().Be(2);
        }

        [Test]
        public void UsesDefaultConstructorIfNecessary()
        {
            var resolver = new OnDemandResolver();
            var result = (TestTypesForIocTesting.ITestClassDefaultCtor)resolver.Resolve(typeof(TestTypesForIocTesting.ITestClassDefaultCtor));
            result.CtorCalled.Should().BeTrue();
        }

        [Test]
        public void ResolvesInstanceOfConcreteConstructorParameter()
        {
            var resolver = new OnDemandResolver();
            var result = (TestTypesForIocTesting.ITestClassMultipleCtor)resolver.Resolve(typeof(TestTypesForIocTesting.ITestClassMultipleCtor));
            result.TestClassInst.Should().NotBeNull();
        }
    }
}
