using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("gamelift", "register-compute")]
public record AwsGameliftRegisterComputeOptions(
[property: CliOption("--fleet-id")] string FleetId,
[property: CliOption("--compute-name")] string ComputeName
) : AwsOptions
{
    [CliOption("--certificate-path")]
    public string? CertificatePath { get; set; }

    [CliOption("--dns-name")]
    public string? DnsName { get; set; }

    [CliOption("--ip-address")]
    public string? IpAddress { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}