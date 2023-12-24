using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kinesisanalyticsv2", "start-application")]
public record AwsKinesisanalyticsv2StartApplicationOptions(
[property: CommandSwitch("--application-name")] string ApplicationName
) : AwsOptions
{
    [CommandSwitch("--run-configuration")]
    public string? RunConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}