using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devops", "security", "group", "create")]
public record AzDevopsSecurityGroupCreateOptions : AzOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--detect")]
    public bool? Detect { get; set; }

    [CommandSwitch("--email-id")]
    public string? EmailId { get; set; }

    [CommandSwitch("--groups")]
    public string? Groups { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--org")]
    public string? Org { get; set; }

    [CommandSwitch("--origin-id")]
    public string? OriginId { get; set; }

    [CommandSwitch("--project")]
    public string? Project { get; set; }

    [CommandSwitch("--scope")]
    public string? Scope { get; set; }
}