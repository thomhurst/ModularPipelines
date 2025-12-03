using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("customer-profiles", "get-auto-merging-preview")]
public record AwsCustomerProfilesGetAutoMergingPreviewOptions(
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--consolidation")] string Consolidation,
[property: CliOption("--conflict-resolution")] string ConflictResolution
) : AwsOptions
{
    [CliOption("--min-allowed-confidence-score-for-merging")]
    public double? MinAllowedConfidenceScoreForMerging { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}