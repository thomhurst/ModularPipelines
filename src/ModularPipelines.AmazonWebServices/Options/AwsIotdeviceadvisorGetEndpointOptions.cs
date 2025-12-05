using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotdeviceadvisor", "get-endpoint")]
public record AwsIotdeviceadvisorGetEndpointOptions : AwsOptions
{
    [CliOption("--thing-arn")]
    public string? ThingArn { get; set; }

    [CliOption("--certificate-arn")]
    public string? CertificateArn { get; set; }

    [CliOption("--device-role-arn")]
    public string? DeviceRoleArn { get; set; }

    [CliOption("--authentication-method")]
    public string? AuthenticationMethod { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}