using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm", "unlabel-parameter-version")]
public record AwsSsmUnlabelParameterVersionOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--parameter-version")] long ParameterVersion,
[property: CommandSwitch("--labels")] string[] Labels
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}