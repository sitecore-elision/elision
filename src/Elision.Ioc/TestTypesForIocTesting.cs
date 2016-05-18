using System.Web.Mvc;
using System.Web.Routing;

namespace Elision.Ioc
{
    public class TestTypesForIocTesting
    {
        public interface ITestClass { }

        public class TestClass : ITestClass { }

        public interface ITestClassMultipleImpl { }

        public class TestClassMultipleImpl1 : ITestClassMultipleImpl { }

        public class TestClassMultipleImpl2 : ITestClassMultipleImpl { }

        public interface ITestClassNoCtor { }

        public class TestClassNoCtor : ITestClassNoCtor { }

        public interface ITestClassDefaultCtor
        {
            bool CtorCalled { get; set; }
        }

        public class TestClassDefaultCtor : ITestClassDefaultCtor
        {
            public bool CtorCalled { get; set; }
            public TestClassDefaultCtor() { CtorCalled = true; }
        }

        public interface ITestClassMultipleCtor
        {
            ITestClass TestClassInst { get; set; }
            int CtorCalled { get; set; }
        }

        public class TestClassMultipleCtor : ITestClassMultipleCtor
        {
            public ITestClass TestClassInst { get; set; }
            public int CtorCalled { get; set; }
            public TestClassMultipleCtor() { CtorCalled = 1; }
            public TestClassMultipleCtor(ITestClass obj)
            {
                TestClassInst = obj;
                CtorCalled = 2;
            }
        }

        public class TestController : IController
        {
            public void Execute(RequestContext requestContext)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
