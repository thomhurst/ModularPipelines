using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("gamelift", "register-compute")]
public record AwsGameliftRegisterComputeOptions(
[property: CommandSwitch("--fleet-id")] string FleetId,
[property: CommandSwitch("--compute-name")] string ComputeName
) : AwsOptions
{
    [CommandSwitch("--certificate-path")]
    public string? CertificatePath { get; set; }

    [CommandSwitch("--dns-name")]
    public string? DnsName { get; set; }

    [CommandSwitch("--ip-address")]
    public string? IpAddress { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}