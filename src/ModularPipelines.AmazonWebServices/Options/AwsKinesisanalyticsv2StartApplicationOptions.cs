using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kinesisanalyticsv2", "start-application")]
public record AwsKinesisanalyticsv2StartApplicationOptions(
[property: CliOption("--application-name")] string ApplicationName
) : AwsOptions
{
    [CliOption("--run-configuration")]
    public string? RunConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}