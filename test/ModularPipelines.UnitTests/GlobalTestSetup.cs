using ModularPipelines.Enums;

namespace ModularPipelines.UnitTests;

[SetUpFixture]
public static class GlobalTestSetup
{
    [OneTimeSetUp]
    public static void Setup()
    {
        GlobalConfig.DefaultCommandLogging = CommandLogging.Error;
    }
    
    [OneTimeTearDown]
    public static void Teardown()
    {
        GC.Collect();
    }
}