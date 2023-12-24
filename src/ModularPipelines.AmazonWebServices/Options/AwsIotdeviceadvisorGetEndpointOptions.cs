using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotdeviceadvisor", "get-endpoint")]
public record AwsIotdeviceadvisorGetEndpointOptions : AwsOptions
{
    [CommandSwitch("--thing-arn")]
    public string? ThingArn { get; set; }

    [CommandSwitch("--certificate-arn")]
    public string? CertificateArn { get; set; }

    [CommandSwitch("--device-role-arn")]
    public string? DeviceRoleArn { get; set; }

    [CommandSwitch("--authentication-method")]
    public string? AuthenticationMethod { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}