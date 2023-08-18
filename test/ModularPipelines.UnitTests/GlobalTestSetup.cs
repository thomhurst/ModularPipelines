namespace ModularPipelines.UnitTests;

[SetUpFixture]
public class GlobalTestSetup
{
    [OneTimeSetUp]
    public static void Setup()
    {
        GlobalConfig.LogCommandInput = false;
        GlobalConfig.LogCommandOutput = false;
        
        Console.SetOut(new StringWriter());
    }
}