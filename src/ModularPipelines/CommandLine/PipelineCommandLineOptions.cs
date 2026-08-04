using ModularPipelines.Enums;

namespace ModularPipelines.PipelineCli;

internal sealed record PipelineCommandLineOptions(
    PipelineCommand Command,
    bool DisableModuleCache,
    IReadOnlyList<string> HostArguments,
    IReadOnlyList<string> TargetModules,
    IReadOnlyList<string> SkippedModules,
    IReadOnlyList<string> RunOnlyCategories,
    IReadOnlyList<string> IgnoreCategories,
    DependencyGraphFormat? GraphFormat,
    string? GraphPath)
{
    public static PipelineCommandLineOptions Empty { get; } = new(
        PipelineCommand.Run,
        false,
        [],
        [],
        [],
        [],
        [],
        null,
        null);
}
