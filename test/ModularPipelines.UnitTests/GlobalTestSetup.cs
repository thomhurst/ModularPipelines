namespace ModularPipelines.UnitTests;

public class GlobalTestSetup
{
    [AssemblySetUp]
    public static void Setup()
    {
        Environment.CurrentDirectory = TestContext.OutputDirectory;
    }
}