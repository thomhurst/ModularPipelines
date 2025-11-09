namespace ModularPipelines.UnitTests.Attributes;

/// <summary>
/// Skips tests that use the SubModule API which was removed in v3.0
/// </summary>
public class SubModuleApiRemovedAttribute() : SkipAttribute("SubModule API removed in v3.0")
{
    public override Task<bool> ShouldSkip(TestRegisteredContext context)
    {
        // Always skip these tests until SubModule API is redesigned
        return Task.FromResult(true);
    }
}
