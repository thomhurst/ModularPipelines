using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml-engine", "local", "train")]
public record GcloudMlEngineLocalTrainOptions(
[property: PositionalArgument] string UserArgs,
[property: CommandSwitch("--module-name")] string ModuleName
) : GcloudOptions
{
    [BooleanCommandSwitch("--distributed")]
    public bool? Distributed { get; set; }

    [CommandSwitch("--evaluator-count")]
    public string? EvaluatorCount { get; set; }

    [CommandSwitch("--job-dir")]
    public string? JobDir { get; set; }

    [CommandSwitch("--package-path")]
    public string? PackagePath { get; set; }

    [CommandSwitch("--parameter-server-count")]
    public string? ParameterServerCount { get; set; }

    [CommandSwitch("--start-port")]
    public string? StartPort { get; set; }

    [CommandSwitch("--worker-count")]
    public string? WorkerCount { get; set; }
}