using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "domain", "list", "(eventgrid", "extension)")]
public record AzEventgridDomainListEventgridExtensionOptions : AzOptions
{
    [CliOption("--odata-query")]
    public string? OdataQuery { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}