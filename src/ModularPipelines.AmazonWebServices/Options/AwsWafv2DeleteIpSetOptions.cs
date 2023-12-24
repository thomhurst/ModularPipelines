using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("wafv2", "delete-ip-set")]
public record AwsWafv2DeleteIpSetOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--scope")] string Scope,
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--lock-token")] string LockToken
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}