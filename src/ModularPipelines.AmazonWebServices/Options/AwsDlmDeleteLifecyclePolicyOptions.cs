using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dlm", "delete-lifecycle-policy")]
public record AwsDlmDeleteLifecyclePolicyOptions(
[property: CommandSwitch("--policy-id")] string PolicyId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}