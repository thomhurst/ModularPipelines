using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kinesisanalyticsv2", "describe-application-version")]
public record AwsKinesisanalyticsv2DescribeApplicationVersionOptions(
[property: CommandSwitch("--application-name")] string ApplicationName,
[property: CommandSwitch("--application-version-id")] long ApplicationVersionId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}