using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lexv2-models", "create-resource-policy")]
public record AwsLexv2ModelsCreateResourcePolicyOptions(
[property: CommandSwitch("--resource-arn")] string ResourceArn,
[property: CommandSwitch("--policy")] string Policy
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}