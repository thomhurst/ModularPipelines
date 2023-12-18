using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apic", "metadata-schema", "create")]
public record AzApicMetadataSchemaCreateOptions(
[property: CommandSwitch("--metadata-schema")] string MetadataSchema,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--service")] string Service
) : AzOptions
{
    [CommandSwitch("--assigned-to")]
    public string? AssignedTo { get; set; }

    [CommandSwitch("--file-name")]
    public string? FileName { get; set; }

    [CommandSwitch("--schema")]
    public string? Schema { get; set; }
}