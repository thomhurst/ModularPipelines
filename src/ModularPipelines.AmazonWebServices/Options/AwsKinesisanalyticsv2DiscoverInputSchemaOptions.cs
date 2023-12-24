using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kinesisanalyticsv2", "discover-input-schema")]
public record AwsKinesisanalyticsv2DiscoverInputSchemaOptions(
[property: CommandSwitch("--service-execution-role")] string ServiceExecutionRole
) : AwsOptions
{
    [CommandSwitch("--resource-arn")]
    public string? ResourceArn { get; set; }

    [CommandSwitch("--input-starting-position-configuration")]
    public string? InputStartingPositionConfiguration { get; set; }

    [CommandSwitch("--s3-configuration")]
    public string? S3Configuration { get; set; }

    [CommandSwitch("--input-processing-configuration")]
    public string? InputProcessingConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}