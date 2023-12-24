using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53", "disassociate-vpc-from-hosted-zone")]
public record AwsRoute53DisassociateVpcFromHostedZoneOptions(
[property: CommandSwitch("--hosted-zone-id")] string HostedZoneId,
[property: CommandSwitch("--vpc")] string Vpc
) : AwsOptions
{
    [CommandSwitch("--comment")]
    public string? Comment { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}