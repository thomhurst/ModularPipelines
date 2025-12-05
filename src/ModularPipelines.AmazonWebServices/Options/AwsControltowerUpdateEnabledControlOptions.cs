using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("controltower", "update-enabled-control")]
public record AwsControltowerUpdateEnabledControlOptions(
[property: CliOption("--enabled-control-identifier")] string EnabledControlIdentifier,
[property: CliOption("--parameters")] string[] Parameters
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}