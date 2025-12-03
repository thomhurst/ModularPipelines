using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("controltower", "enable-control")]
public record AwsControltowerEnableControlOptions(
[property: CliOption("--control-identifier")] string ControlIdentifier,
[property: CliOption("--target-identifier")] string TargetIdentifier
) : AwsOptions
{
    [CliOption("--parameters")]
    public string[]? Parameters { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}