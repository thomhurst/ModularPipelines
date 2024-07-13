namespace ModularPipelines.UnitTests.Attributes;

public class LinuxOnlyTestAttribute() : SkipAttribute("Linux only test")
{
    public override Task<bool> ShouldSkip(TestContext testContext)
    {
        return Task.FromResult(!OperatingSystem.IsLinux());
    }
}