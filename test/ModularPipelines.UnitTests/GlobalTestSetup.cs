namespace ModularPipelines.UnitTests;

[SetUpFixture]
public static class GlobalTestSetup
{
    [OneTimeTearDown]
    public static void Teardown()
    {
        GC.Collect();
    }
}