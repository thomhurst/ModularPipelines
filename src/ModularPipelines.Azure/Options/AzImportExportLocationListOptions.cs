using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("import-export", "location", "list")]
public record AzImportExportLocationListOptions(
[property: CommandSwitch("--location")] string Location
) : AzOptions
{
}

