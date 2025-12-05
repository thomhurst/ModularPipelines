using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("controltower", "get-enabled-control")]
public record AwsControltowerGetEnabledControlOptions(
[property: CliOption("--enabled-control-identifier")] string EnabledControlIdentifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}