using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm", "create-patch-baseline")]
public record AwsSsmCreatePatchBaselineOptions(
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--operating-system")]
    public string? OperatingSystem { get; set; }

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

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}