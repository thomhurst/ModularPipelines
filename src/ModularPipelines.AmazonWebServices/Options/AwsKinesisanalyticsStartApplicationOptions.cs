using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kinesisanalytics", "start-application")]
public record AwsKinesisanalyticsStartApplicationOptions(
[property: CliOption("--application-name")] string ApplicationName,
[property: CliOption("--input-configurations")] string[] InputConfigurations
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}