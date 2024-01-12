using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("composer", "environments", "storage", "dags", "export")]
public record GcloudComposerEnvironmentsStorageDagsExportOptions(
[property: CommandSwitch("--destination")] string Destination,
[property: CommandSwitch("--environment")] string Environment,
[property: CommandSwitch("--location")] string Location
) : GcloudOptions
{
    [CommandSwitch("--source")]
    public string? Source { get; set; }
}