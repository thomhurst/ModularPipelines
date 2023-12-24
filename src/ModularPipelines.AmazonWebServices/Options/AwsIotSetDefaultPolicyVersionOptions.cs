using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "set-default-policy-version")]
public record AwsIotSetDefaultPolicyVersionOptions(
[property: CommandSwitch("--policy-name")] string PolicyName,
[property: CommandSwitch("--policy-version-id")] string PolicyVersionId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}