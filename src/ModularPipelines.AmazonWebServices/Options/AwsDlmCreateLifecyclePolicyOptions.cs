using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dlm", "create-lifecycle-policy")]
public record AwsDlmCreateLifecyclePolicyOptions(
[property: CommandSwitch("--execution-role-arn")] string ExecutionRoleArn,
[property: CommandSwitch("--description")] string Description,
[property: CommandSwitch("--state")] string State
) : AwsOptions
{
    [CommandSwitch("--policy-details")]
    public string? PolicyDetails { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--default-policy")]
    public string? DefaultPolicy { get; set; }

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