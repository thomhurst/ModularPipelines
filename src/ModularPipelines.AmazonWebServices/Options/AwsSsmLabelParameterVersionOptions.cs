using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm", "label-parameter-version")]
public record AwsSsmLabelParameterVersionOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--labels")] string[] Labels
) : AwsOptions
{
    [CommandSwitch("--parameter-version")]
    public long? ParameterVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}