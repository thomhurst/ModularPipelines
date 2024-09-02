namespace ModularPipelines.UnitTests.Attributes;

public class WindowsOnlyTestAttribute() : SkipAttribute("Windows only test")
{
    public override Task<bool> ShouldSkip(BeforeTestContext context)
    {
        return Task.FromResult(!OperatingSystem.IsWindows());
    }
}