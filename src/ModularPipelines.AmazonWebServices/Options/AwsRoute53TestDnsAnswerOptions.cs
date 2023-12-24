using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53", "test-dns-answer")]
public record AwsRoute53TestDnsAnswerOptions(
[property: CommandSwitch("--hosted-zone-id")] string HostedZoneId,
[property: CommandSwitch("--record-name")] string RecordName,
[property: CommandSwitch("--record-type")] string RecordType
) : AwsOptions
{
    [CommandSwitch("--resolver-ip")]
    public string? ResolverIp { get; set; }

    [CommandSwitch("--edns0-client-subnet-ip")]
    public string? Edns0ClientSubnetIp { get; set; }

    [CommandSwitch("--edns0-client-subnet-mask")]
    public string? Edns0ClientSubnetMask { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}