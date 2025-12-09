using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("eventgrid", "partner", "verified-partner", "list")]
public record AzEventgridPartnerVerifiedPartnerListOptions : AzOptions
{
    [CliOption("--odata-query")]
    public string? OdataQuery { get; set; }
}