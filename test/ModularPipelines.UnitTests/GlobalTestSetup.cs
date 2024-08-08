namespace ModularPipelines.UnitTests;

public static class GlobalTestSetup
{
    [Before(Assembly)]
    public static void Setup()
    {
        Environment.CurrentDirectory = TestContext.OutputDirectory;
    }
}