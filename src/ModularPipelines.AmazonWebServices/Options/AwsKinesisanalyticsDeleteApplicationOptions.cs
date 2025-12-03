using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kinesisanalytics", "delete-application")]
public record AwsKinesisanalyticsDeleteApplicationOptions(
[property: CliOption("--application-name")] string ApplicationName,
[property: CliOption("--create-timestamp")] long CreateTimestamp
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}