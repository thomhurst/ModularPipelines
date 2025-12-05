using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("globalaccelerator", "update-accelerator-attributes")]
public record AwsGlobalacceleratorUpdateAcceleratorAttributesOptions(
[property: CliOption("--accelerator-arn")] string AcceleratorArn
) : AwsOptions
{
    [CliOption("--flow-logs-s3-bucket")]
    public string? FlowLogsS3Bucket { get; set; }

    [CliOption("--flow-logs-s3-prefix")]
    public string? FlowLogsS3Prefix { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}