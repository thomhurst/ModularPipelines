namespace ModularPipelines.UnitTests;

public static class GlobalTestSetup
{
    [AssemblySetUp]
    public static void Setup()
    {
        Environment.CurrentDirectory = TestContext.OutputDirectory;
    }
}