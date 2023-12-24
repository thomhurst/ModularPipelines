using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "describe-workteam")]
public record AwsSagemakerDescribeWorkteamOptions(
[property: CommandSwitch("--workteam-name")] string WorkteamName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}