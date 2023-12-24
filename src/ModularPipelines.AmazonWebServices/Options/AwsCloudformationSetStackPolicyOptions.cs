using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudformation", "set-stack-policy")]
public record AwsCloudformationSetStackPolicyOptions(
[property: CommandSwitch("--stack-name")] string StackName
) : AwsOptions
{
    [CommandSwitch("--stack-policy-body")]
    public string? StackPolicyBody { get; set; }

    [CommandSwitch("--stack-policy-url")]
    public string? StackPolicyUrl { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}