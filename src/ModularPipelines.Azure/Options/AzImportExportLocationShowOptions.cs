using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("import-export", "location", "show")]
public record AzImportExportLocationShowOptions(
[property: CommandSwitch("--location")] string Location
) : AzOptions
{
}