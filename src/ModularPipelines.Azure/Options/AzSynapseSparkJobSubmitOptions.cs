using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("synapse", "spark", "job", "submit")]
public record AzSynapseSparkJobSubmitOptions(
[property: CliOption("--executor-size")] string ExecutorSize,
[property: CliOption("--executors")] string Executors,
[property: CliOption("--main-definition-file")] string MainDefinitionFile,
[property: CliOption("--name")] string Name,
[property: CliOption("--spark-pool-name")] string SparkPoolName,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliOption("--archives")]
    public string? Archives { get; set; }

    [CliOption("--arguments")]
    public string? SynapseSparkJobSubmitArguments { get; set; }

    [CliOption("--configuration")]
    public string? Configuration { get; set; }

    [CliOption("--language")]
    public string? Language { get; set; }

    [CliOption("--main-class-name")]
    public string? MainClassName { get; set; }

    [CliOption("--reference-files")]
    public string? ReferenceFiles { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}