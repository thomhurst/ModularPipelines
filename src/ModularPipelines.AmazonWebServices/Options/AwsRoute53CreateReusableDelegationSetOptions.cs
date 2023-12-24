using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53", "create-reusable-delegation-set")]
public record AwsRoute53CreateReusableDelegationSetOptions(
[property: CommandSwitch("--caller-reference")] string CallerReference
) : AwsOptions
{
    [CommandSwitch("--hosted-zone-id")]
    public string? HostedZoneId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}