using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("eventgrid", "system-topic", "list")]
public record AzEventgridSystemTopicListOptions : AzOptions
{
    [CliOption("--odata-query")]
    public string? OdataQuery { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}