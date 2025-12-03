using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dlm", "create-lifecycle-policy")]
public record AwsDlmCreateLifecyclePolicyOptions(
[property: CliOption("--execution-role-arn")] string ExecutionRoleArn,
[property: CliOption("--description")] string Description,
[property: CliOption("--state")] string State
) : AwsOptions
{
    [CliOption("--policy-details")]
    public string? PolicyDetails { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--default-policy")]
    public string? DefaultPolicy { get; set; }

    [CliOption("--create-interval")]
    public int? CreateInterval { get; set; }

    [CliOption("--retain-interval")]
    public int? RetainInterval { get; set; }

    [CliOption("--cross-region-copy-targets")]
    public string[]? CrossRegionCopyTargets { get; set; }

    [CliOption("--exclusions")]
    public string? Exclusions { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}