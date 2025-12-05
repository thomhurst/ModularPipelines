using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dlm", "get-lifecycle-policies")]
public record AwsDlmGetLifecyclePoliciesOptions : AwsOptions
{
    [CliOption("--policy-ids")]
    public string[]? PolicyIds { get; set; }

    [CliOption("--state")]
    public string? State { get; set; }

    [CliOption("--resource-types")]
    public string[]? ResourceTypes { get; set; }

    [CliOption("--target-tags")]
    public string[]? TargetTags { get; set; }

    [CliOption("--tags-to-add")]
    public string[]? TagsToAdd { get; set; }

    [CliOption("--default-policy-type")]
    public string? DefaultPolicyType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}