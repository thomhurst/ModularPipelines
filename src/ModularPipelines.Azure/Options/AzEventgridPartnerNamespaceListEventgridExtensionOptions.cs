using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "partner", "namespace", "list", "(eventgrid", "extension)")]
public record AzEventgridPartnerNamespaceListEventgridExtensionOptions : AzOptions
{
    [CommandSwitch("--odata-query")]
    public string? OdataQuery { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}

