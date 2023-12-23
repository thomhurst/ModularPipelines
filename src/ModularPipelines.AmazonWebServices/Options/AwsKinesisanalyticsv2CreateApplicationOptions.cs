using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kinesisanalyticsv2", "create-application")]
public record AwsKinesisanalyticsv2CreateApplicationOptions(
[property: CommandSwitch("--application-name")] string ApplicationName,
[property: CommandSwitch("--runtime-environment")] string RuntimeEnvironment,
[property: CommandSwitch("--service-execution-role")] string ServiceExecutionRole
) : AwsOptions
{
    [CommandSwitch("--application-description")]
    public string? ApplicationDescription { get; set; }

    [CommandSwitch("--application-configuration")]
    public string? ApplicationConfiguration { get; set; }

    [CommandSwitch("--cloud-watch-logging-options")]
    public string[]? CloudWatchLoggingOptions { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--application-mode")]
    public string? ApplicationMode { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}