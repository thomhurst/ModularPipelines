using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicediscovery", "update-instance-custom-health-status")]
public record AwsServicediscoveryUpdateInstanceCustomHealthStatusOptions(
[property: CommandSwitch("--service-id")] string ServiceId,
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--status")] string Status
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}