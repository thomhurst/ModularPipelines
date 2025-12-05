using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53", "test-dns-answer")]
public record AwsRoute53TestDnsAnswerOptions(
[property: CliOption("--hosted-zone-id")] string HostedZoneId,
[property: CliOption("--record-name")] string RecordName,
[property: CliOption("--record-type")] string RecordType
) : AwsOptions
{
    [CliOption("--resolver-ip")]
    public string? ResolverIp { get; set; }

    [CliOption("--edns0-client-subnet-ip")]
    public string? Edns0ClientSubnetIp { get; set; }

    [CliOption("--edns0-client-subnet-mask")]
    public string? Edns0ClientSubnetMask { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}