using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("controltower", "disable-control")]
public record AwsControltowerDisableControlOptions(
[property: CliOption("--control-identifier")] string ControlIdentifier,
[property: CliOption("--target-identifier")] string TargetIdentifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}