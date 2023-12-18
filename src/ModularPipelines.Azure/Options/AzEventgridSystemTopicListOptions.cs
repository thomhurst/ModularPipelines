using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "system-topic", "list")]
public record AzEventgridSystemTopicListOptions : AzOptions
{
    [CommandSwitch("--odata-query")]
    public string? OdataQuery { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}