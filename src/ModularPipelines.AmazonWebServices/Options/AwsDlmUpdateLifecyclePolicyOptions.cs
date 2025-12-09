using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dlm", "update-lifecycle-policy")]
public record AwsDlmUpdateLifecyclePolicyOptions(
[property: CliOption("--policy-id")] string PolicyId
) : AwsOptions
{
    [CliOption("--execution-role-arn")]
    public string? ExecutionRoleArn { get; set; }

    [CliOption("--state")]
    public string? State { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--policy-details")]
    public string? PolicyDetails { get; set; }

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