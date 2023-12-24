using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53", "get-hosted-zone-limit")]
public record AwsRoute53GetHostedZoneLimitOptions(
[property: CommandSwitch("--type")] string Type,
[property: CommandSwitch("--hosted-zone-id")] string HostedZoneId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}