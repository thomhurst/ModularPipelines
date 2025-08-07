namespace ModularPipelines.UnitTests.Attributes;

public class WindowsOnlyTestAttribute() : SkipAttribute("Windows only test")
{
    public override Task<bool> ShouldSkip(TestRegisteredContext testRegisteredContext)
    {
        return Task.FromResult(!OperatingSystem.IsWindows());
    }
}