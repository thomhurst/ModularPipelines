using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "partner", "verified-partner", "list")]
public record AzEventgridPartnerVerifiedPartnerListOptions : AzOptions
{
    [CommandSwitch("--odata-query")]
    public string? OdataQuery { get; set; }
}