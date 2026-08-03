namespace ModularPipelines.TestHelpers;

public static class GlobalTestSetup
{
    [Before(TestDiscovery)]
    public static void Setup()
    {
        Environment.CurrentDirectory = TestContext.OutputDirectory!;
    }
}
