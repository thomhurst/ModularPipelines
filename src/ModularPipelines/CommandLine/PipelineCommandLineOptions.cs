using ModularPipelines.Enums;
using ModularPipelines.Reporting;

namespace ModularPipelines.PipelineCli;

internal sealed record PipelineCommandLineOptions(
    PipelineCommand Command,
    bool DisableModuleCache,
    IReadOnlyList<string> HostArguments,
    IReadOnlyList<string> RunOnlyModules,
    IReadOnlyList<string> IgnoreModules,
    IReadOnlyList<string> RunOnlyCategories,
    IReadOnlyList<string> IgnoreCategories,
    IReadOnlyList<string> RunOnlyTags,
    IReadOnlyList<string> IgnoreTags,
    DependencyGraphFormat? GraphFormat,
    string? GraphPath,
    bool RerunFailed = false)
{
    public static PipelineCommandLineOptions Empty { get; } = new(
        PipelineCommand.Run,
        false,
        [],
        [],
        [],
        [],
        [],
        [],
        [],
        null,
        null);
}
