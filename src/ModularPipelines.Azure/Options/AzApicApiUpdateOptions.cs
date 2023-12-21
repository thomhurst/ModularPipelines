using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apic", "api", "update")]
public record AzApicApiUpdateOptions : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--api")]
    public string? Api { get; set; }

    [CommandSwitch("--contacts")]
    public string? Contacts { get; set; }

    [CommandSwitch("--custom-properties")]
    public string? CustomProperties { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--external-documentation")]
    public string? ExternalDocumentation { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--kind")]
    public string? Kind { get; set; }

    [CommandSwitch("--license")]
    public string? License { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--service")]
    public string? Service { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }

    [CommandSwitch("--summary")]
    public string? Summary { get; set; }

    [CommandSwitch("--terms-of-service")]
    public string? TermsOfService { get; set; }

    [CommandSwitch("--title")]
    public string? Title { get; set; }

    [CommandSwitch("--workspace")]
    public string? Workspace { get; set; }
}