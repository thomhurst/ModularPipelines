using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("neptunedata", "get-rdf-graph-summary")]
public record AwsNeptunedataGetRdfGraphSummaryOptions : AwsOptions
{
    [CommandSwitch("--mode")]
    public string? Mode { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}