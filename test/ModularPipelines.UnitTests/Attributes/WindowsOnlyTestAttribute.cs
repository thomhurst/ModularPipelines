namespace ModularPipelines.UnitTests.Attributes;

public class WindowsOnlyTestAttribute() : SkipAttribute("Windows only test")
{
    public override Task<bool> ShouldSkip(TestRegisteredContext context)
    {
        return Task.FromResult(!OperatingSystem.IsWindows());
    }
}