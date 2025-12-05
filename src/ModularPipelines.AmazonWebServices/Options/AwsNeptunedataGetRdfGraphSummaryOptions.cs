using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("neptunedata", "get-rdf-graph-summary")]
public record AwsNeptunedataGetRdfGraphSummaryOptions : AwsOptions
{
    [CliOption("--mode")]
    public string? Mode { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}