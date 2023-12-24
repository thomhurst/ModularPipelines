using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm", "update-patch-baseline")]
public record AwsSsmUpdatePatchBaselineOptions(
[property: CommandSwitch("--baseline-id")] string BaselineId
) : AwsOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--global-filters")]
    public string? GlobalFilters { get; set; }

    [CommandSwitch("--approval-rules")]
    public string? ApprovalRules { get; set; }

    [CommandSwitch("--approved-patches")]
    public string[]? ApprovedPatches { get; set; }

    [CommandSwitch("--approved-patches-compliance-level")]
    public string? ApprovedPatchesComplianceLevel { get; set; }

    [CommandSwitch("--rejected-patches")]
    public string[]? RejectedPatches { get; set; }

    [CommandSwitch("--rejected-patches-action")]
    public string? RejectedPatchesAction { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--sources")]
    public string[]? Sources { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}