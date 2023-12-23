using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dlm", "get-lifecycle-policies")]
public record AwsDlmGetLifecyclePoliciesOptions : AwsOptions
{
    [CommandSwitch("--policy-ids")]
    public string[]? PolicyIds { get; set; }

    [CommandSwitch("--state")]
    public string? State { get; set; }

    [CommandSwitch("--resource-types")]
    public string[]? ResourceTypes { get; set; }

    [CommandSwitch("--target-tags")]
    public string[]? TargetTags { get; set; }

    [CommandSwitch("--tags-to-add")]
    public string[]? TagsToAdd { get; set; }

    [CommandSwitch("--default-policy-type")]
    public string? DefaultPolicyType { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}