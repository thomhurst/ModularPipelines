using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("customer-profiles", "get-auto-merging-preview")]
public record AwsCustomerProfilesGetAutoMergingPreviewOptions(
[property: CommandSwitch("--domain-name")] string DomainName,
[property: CommandSwitch("--consolidation")] string Consolidation,
[property: CommandSwitch("--conflict-resolution")] string ConflictResolution
) : AwsOptions
{
    [CommandSwitch("--min-allowed-confidence-score-for-merging")]
    public double? MinAllowedConfidenceScoreForMerging { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}