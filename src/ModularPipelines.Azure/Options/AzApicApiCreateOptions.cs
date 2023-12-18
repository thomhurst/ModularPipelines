using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apic", "api", "create")]
public record AzApicApiCreateOptions(
[property: CommandSwitch("--api")] string Api,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--service")] string Service
) : AzOptions
{
    [CommandSwitch("--contacts")]
    public string? Contacts { get; set; }

    [CommandSwitch("--custom-properties")]
    public string? CustomProperties { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--external-documentation")]
    public string? ExternalDocumentation { get; set; }

    [CommandSwitch("--kind")]
    public string? Kind { get; set; }

    [CommandSwitch("--license")]
    public string? License { get; set; }

    [CommandSwitch("--summary")]
    public string? Summary { get; set; }

    [CommandSwitch("--terms-of-service")]
    public string? TermsOfService { get; set; }

    [CommandSwitch("--title")]
    public string? Title { get; set; }

    [CommandSwitch("--workspace")]
    public string? Workspace { get; set; }
}