using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kinesisanalytics", "create-application")]
public record AwsKinesisanalyticsCreateApplicationOptions(
[property: CommandSwitch("--application-name")] string ApplicationName
) : AwsOptions
{
    [CommandSwitch("--application-description")]
    public string? ApplicationDescription { get; set; }

    [CommandSwitch("--inputs")]
    public string[]? Inputs { get; set; }

    [CommandSwitch("--outputs")]
    public string[]? Outputs { get; set; }

    [CommandSwitch("--cloud-watch-logging-options")]
    public string[]? CloudWatchLoggingOptions { get; set; }

    [CommandSwitch("--application-code")]
    public string? ApplicationCode { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}