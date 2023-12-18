using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apic", "metadata-schema", "head")]
public record AzApicMetadataSchemaHeadOptions : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--metadata-schema")]
    public string? MetadataSchema { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--service")]
    public string? Service { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}