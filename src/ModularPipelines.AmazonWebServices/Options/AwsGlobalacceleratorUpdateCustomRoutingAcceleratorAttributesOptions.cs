using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("globalaccelerator", "update-custom-routing-accelerator-attributes")]
public record AwsGlobalacceleratorUpdateCustomRoutingAcceleratorAttributesOptions(
[property: CommandSwitch("--accelerator-arn")] string AcceleratorArn
) : AwsOptions
{
    [CommandSwitch("--flow-logs-s3-bucket")]
    public string? FlowLogsS3Bucket { get; set; }

    [CommandSwitch("--flow-logs-s3-prefix")]
    public string? FlowLogsS3Prefix { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}