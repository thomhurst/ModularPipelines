using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("xray", "put-telemetry-records")]
public record AwsXrayPutTelemetryRecordsOptions(
[property: CliOption("--telemetry-records")] string[] TelemetryRecords
) : AwsOptions
{
    [CliOption("--ec2-instance-id")]
    public string? Ec2InstanceId { get; set; }

    [CliOption("--hostname")]
    public string? Hostname { get; set; }

    [CliOption("--resource-arn")]
    public string? ResourceArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}