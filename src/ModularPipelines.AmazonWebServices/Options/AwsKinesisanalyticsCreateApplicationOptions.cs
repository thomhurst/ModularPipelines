using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kinesisanalytics", "create-application")]
public record AwsKinesisanalyticsCreateApplicationOptions(
[property: CliOption("--application-name")] string ApplicationName
) : AwsOptions
{
    [CliOption("--application-description")]
    public string? ApplicationDescription { get; set; }

    [CliOption("--inputs")]
    public string[]? Inputs { get; set; }

    [CliOption("--outputs")]
    public string[]? Outputs { get; set; }

    [CliOption("--cloud-watch-logging-options")]
    public string[]? CloudWatchLoggingOptions { get; set; }

    [CliOption("--application-code")]
    public string? ApplicationCode { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}