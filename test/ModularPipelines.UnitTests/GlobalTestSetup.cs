namespace ModularPipelines.UnitTests;

[SetUpFixture]
public static class GlobalTestSetup
{
    [OneTimeSetUp]
    public static void Setup()
    {
        Environment.SetEnvironmentVariable("GITHUB_ACTIONS", null);
    }
    
    [OneTimeTearDown]
    public static void Teardown()
    {
        GC.Collect();
    }
}