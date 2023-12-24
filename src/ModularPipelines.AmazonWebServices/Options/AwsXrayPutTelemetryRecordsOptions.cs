using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("xray", "put-telemetry-records")]
public record AwsXrayPutTelemetryRecordsOptions(
[property: CommandSwitch("--telemetry-records")] string[] TelemetryRecords
) : AwsOptions
{
    [CommandSwitch("--ec2-instance-id")]
    public string? Ec2InstanceId { get; set; }

    [CommandSwitch("--hostname")]
    public string? Hostname { get; set; }

    [CommandSwitch("--resource-arn")]
    public string? ResourceArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}