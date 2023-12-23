using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53", "create-hosted-zone")]
public record AwsRoute53CreateHostedZoneOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--caller-reference")] string CallerReference
) : AwsOptions
{
    [CommandSwitch("--vpc")]
    public string? Vpc { get; set; }

    [CommandSwitch("--hosted-zone-config")]
    public string? HostedZoneConfig { get; set; }

    [CommandSwitch("--delegation-set-id")]
    public string? DelegationSetId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}