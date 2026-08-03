namespace ModularPipelines.TestHelpers;

public static class ExternalConfigurationState
{
    public static bool IncludeDependency { get; set; }

    public static bool ShouldIncludeDependency() => IncludeDependency;
}
