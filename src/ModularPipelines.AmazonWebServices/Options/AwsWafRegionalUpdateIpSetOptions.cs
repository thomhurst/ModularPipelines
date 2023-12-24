using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("waf-regional", "update-ip-set")]
public record AwsWafRegionalUpdateIpSetOptions(
[property: CommandSwitch("--ip-set-id")] string IpSetId,
[property: CommandSwitch("--change-token")] string ChangeToken,
[property: CommandSwitch("--updates")] string[] Updates
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}