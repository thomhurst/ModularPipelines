using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("afd", "origin", "show")]
public record AzAfdOriginShowOptions : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--origin-group-name")]
    public string? OriginGroupName { get; set; }

    [CommandSwitch("--origin-name")]
    public string? OriginName { get; set; }

    [CommandSwitch("--profile-name")]
    public string? ProfileName { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}