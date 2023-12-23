using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("controltower", "disable-control")]
public record AwsControltowerDisableControlOptions(
[property: CommandSwitch("--control-identifier")] string ControlIdentifier,
[property: CommandSwitch("--target-identifier")] string TargetIdentifier
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}