using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synapse", "spark", "job", "submit")]
public record AzSynapseSparkJobSubmitOptions(
[property: CommandSwitch("--executor-size")] string ExecutorSize,
[property: CommandSwitch("--executors")] string Executors,
[property: CommandSwitch("--main-definition-file")] string MainDefinitionFile,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--spark-pool-name")] string SparkPoolName,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CommandSwitch("--archives")]
    public string? Archives { get; set; }

    [CommandSwitch("--arguments")]
    public string? SynapseSparkJobSubmitArguments { get; set; }

    [CommandSwitch("--configuration")]
    public string? Configuration { get; set; }

    [CommandSwitch("--language")]
    public string? Language { get; set; }

    [CommandSwitch("--main-class-name")]
    public string? MainClassName { get; set; }

    [CommandSwitch("--reference-files")]
    public string? ReferenceFiles { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}