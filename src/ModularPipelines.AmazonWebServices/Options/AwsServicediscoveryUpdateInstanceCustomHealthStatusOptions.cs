using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("servicediscovery", "update-instance-custom-health-status")]
public record AwsServicediscoveryUpdateInstanceCustomHealthStatusOptions(
[property: CliOption("--service-id")] string ServiceId,
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--status")] string Status
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}