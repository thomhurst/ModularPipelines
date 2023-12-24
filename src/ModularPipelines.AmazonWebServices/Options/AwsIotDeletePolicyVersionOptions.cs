using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "delete-policy-version")]
public record AwsIotDeletePolicyVersionOptions(
[property: CommandSwitch("--policy-name")] string PolicyName,
[property: CommandSwitch("--policy-version-id")] string PolicyVersionId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}