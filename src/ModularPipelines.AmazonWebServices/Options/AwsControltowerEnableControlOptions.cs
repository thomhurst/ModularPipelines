using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("controltower", "enable-control")]
public record AwsControltowerEnableControlOptions(
[property: CommandSwitch("--control-identifier")] string ControlIdentifier,
[property: CommandSwitch("--target-identifier")] string TargetIdentifier
) : AwsOptions
{
    [CommandSwitch("--parameters")]
    public string[]? Parameters { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}