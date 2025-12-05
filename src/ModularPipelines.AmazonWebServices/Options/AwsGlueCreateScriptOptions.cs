using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "create-script")]
public record AwsGlueCreateScriptOptions : AwsOptions
{
    [CliOption("--dag-nodes")]
    public string[]? DagNodes { get; set; }

    [CliOption("--dag-edges")]
    public string[]? DagEdges { get; set; }

    [CliOption("--language")]
    public string? Language { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}