using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kinesisanalyticsv2", "delete-application")]
public record AwsKinesisanalyticsv2DeleteApplicationOptions(
[property: CommandSwitch("--application-name")] string ApplicationName,
[property: CommandSwitch("--create-timestamp")] long CreateTimestamp
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}