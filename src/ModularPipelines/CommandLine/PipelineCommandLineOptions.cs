namespace ModularPipelines.PipelineCli;

internal sealed record PipelineCommandLineOptions(
    PipelineCommand Command,
    bool DisableModuleCache,
    IReadOnlyList<string> HostArguments,
    IReadOnlyList<string> TargetModules,
    IReadOnlyList<string> SkippedModules,
    IReadOnlyList<string> RunOnlyCategories,
    IReadOnlyList<string> IgnoreCategories)
{
    public static PipelineCommandLineOptions Empty { get; } = new(
        PipelineCommand.Run,
        false,
        [],
        [],
        [],
        [],
        []);
}
