namespace ModularPipelines.UnitTests.Attributes;

public class LinuxOnlyTestAttribute() : SkipAttribute("Linux only test")
{
    public override Task<bool> ShouldSkip(BeforeTestContext context)
    {
        return Task.FromResult(!OperatingSystem.IsLinux());
    }
}