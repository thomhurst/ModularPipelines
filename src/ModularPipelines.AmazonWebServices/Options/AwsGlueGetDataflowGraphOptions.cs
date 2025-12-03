using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "get-dataflow-graph")]
public record AwsGlueGetDataflowGraphOptions : AwsOptions
{
    [CliOption("--python-script")]
    public string? PythonScript { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}