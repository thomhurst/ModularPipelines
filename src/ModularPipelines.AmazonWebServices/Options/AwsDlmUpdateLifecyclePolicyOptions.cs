using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dlm", "update-lifecycle-policy")]
public record AwsDlmUpdateLifecyclePolicyOptions(
[property: CommandSwitch("--policy-id")] string PolicyId
) : AwsOptions
{
    [CommandSwitch("--execution-role-arn")]
    public string? ExecutionRoleArn { get; set; }

    [CommandSwitch("--state")]
    public string? State { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--policy-details")]
    public string? PolicyDetails { get; set; }

    [CommandSwitch("--create-interval")]
    public int? CreateInterval { get; set; }

    [CommandSwitch("--retain-interval")]
    public int? RetainInterval { get; set; }

    [CommandSwitch("--cross-region-copy-targets")]
    public string[]? CrossRegionCopyTargets { get; set; }

    [CommandSwitch("--exclusions")]
    public string? Exclusions { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}