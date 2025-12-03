using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudformation", "set-stack-policy")]
public record AwsCloudformationSetStackPolicyOptions(
[property: CliOption("--stack-name")] string StackName
) : AwsOptions
{
    [CliOption("--stack-policy-body")]
    public string? StackPolicyBody { get; set; }

    [CliOption("--stack-policy-url")]
    public string? StackPolicyUrl { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}