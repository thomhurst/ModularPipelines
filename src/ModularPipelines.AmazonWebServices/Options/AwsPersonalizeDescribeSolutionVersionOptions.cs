using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("personalize", "describe-solution-version")]
public record AwsPersonalizeDescribeSolutionVersionOptions(
[property: CommandSwitch("--solution-version-arn")] string SolutionVersionArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}