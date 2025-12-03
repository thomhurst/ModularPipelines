using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codepipeline", "list-action-types")]
public record AwsCodepipelineListActionTypesOptions : AwsOptions
{
    [CliOption("--action-owner-filter")]
    public string? ActionOwnerFilter { get; set; }

    [CliOption("--region-filter")]
    public string? RegionFilter { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}