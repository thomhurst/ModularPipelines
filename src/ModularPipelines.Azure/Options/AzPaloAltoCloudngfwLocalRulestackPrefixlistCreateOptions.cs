using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("palo-alto", "cloudngfw", "local-rulestack", "prefixlist", "create")]
public record AzPaloAltoCloudngfwLocalRulestackPrefixlistCreateOptions(
[property: CommandSwitch("--local-rulestack-name")] string LocalRulestackName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--prefix-list")] string PrefixList,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--audit-comment")]
    public string? AuditComment { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--etag")]
    public string? Etag { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }
}