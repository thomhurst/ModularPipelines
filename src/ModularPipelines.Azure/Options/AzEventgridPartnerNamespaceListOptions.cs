using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "partner", "namespace", "list")]
public record AzEventgridPartnerNamespaceListOptions : AzOptions
{
    [CommandSwitch("--odata-query")]
    public string? OdataQuery { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}