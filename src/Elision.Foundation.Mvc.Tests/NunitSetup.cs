using NUnit.Framework;
using System;
using System.IO;


[SetUpFixture]
public class NunitSetup
{
    public NunitSetup() { }

    [OneTimeSetUp]
    public void SetRunDirectory()
    {          
            
        Environment.CurrentDirectory = TestContext.CurrentContext.TestDirectory;
        // or identically under the hoods
        Directory.SetCurrentDirectory(TestContext.CurrentContext.TestDirectory);
    }

    [OneTimeTearDown]
    public void TeardownSetRunDirectory(){}
}
