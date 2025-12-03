using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "topic", "list")]
public record AzEventgridTopicListOptions : AzOptions
{
    [CliOption("--odata-query")]
    public string? OdataQuery { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}