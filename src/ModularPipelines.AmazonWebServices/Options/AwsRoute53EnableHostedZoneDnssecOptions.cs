using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53", "enable-hosted-zone-dnssec")]
public record AwsRoute53EnableHostedZoneDnssecOptions(
[property: CommandSwitch("--hosted-zone-id")] string HostedZoneId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}