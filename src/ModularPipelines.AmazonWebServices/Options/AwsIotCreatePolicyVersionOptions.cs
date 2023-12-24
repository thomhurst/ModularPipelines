using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "create-policy-version")]
public record AwsIotCreatePolicyVersionOptions(
[property: CommandSwitch("--policy-name")] string PolicyName,
[property: CommandSwitch("--policy-document")] string PolicyDocument
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}