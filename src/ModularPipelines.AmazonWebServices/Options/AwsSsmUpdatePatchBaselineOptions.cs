using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm", "update-patch-baseline")]
public record AwsSsmUpdatePatchBaselineOptions(
[property: CliOption("--baseline-id")] string BaselineId
) : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--global-filters")]
    public string? GlobalFilters { get; set; }

    [CliOption("--approval-rules")]
    public string? ApprovalRules { get; set; }

    [CliOption("--approved-patches")]
    public string[]? ApprovedPatches { get; set; }

    [CliOption("--approved-patches-compliance-level")]
    public string? ApprovedPatchesComplianceLevel { get; set; }

    [CliOption("--rejected-patches")]
    public string[]? RejectedPatches { get; set; }

    [CliOption("--rejected-patches-action")]
    public string? RejectedPatchesAction { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--sources")]
    public string[]? Sources { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}