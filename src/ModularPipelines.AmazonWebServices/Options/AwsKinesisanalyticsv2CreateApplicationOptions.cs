using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kinesisanalyticsv2", "create-application")]
public record AwsKinesisanalyticsv2CreateApplicationOptions(
[property: CliOption("--application-name")] string ApplicationName,
[property: CliOption("--runtime-environment")] string RuntimeEnvironment,
[property: CliOption("--service-execution-role")] string ServiceExecutionRole
) : AwsOptions
{
    [CliOption("--application-description")]
    public string? ApplicationDescription { get; set; }

    [CliOption("--application-configuration")]
    public string? ApplicationConfiguration { get; set; }

    [CliOption("--cloud-watch-logging-options")]
    public string[]? CloudWatchLoggingOptions { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--application-mode")]
    public string? ApplicationMode { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}