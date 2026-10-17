namespace ModularPipelines.UnitTests.Attributes;

public class UnixOnlyTestAttribute() : SkipAttribute("Unix only test")
{
    public override Task<bool> ShouldSkip(TestRegisteredContext context)
    {
        return Task.FromResult(OperatingSystem.IsWindows());
    }
}
