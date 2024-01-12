using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("composer", "environments", "storage", "dags", "import")]
public record GcloudComposerEnvironmentsStorageDagsImportOptions(
[property: CommandSwitch("--source")] string Source,
[property: CommandSwitch("--environment")] string Environment,
[property: CommandSwitch("--location")] string Location
) : GcloudOptions
{
    [CommandSwitch("--destination")]
    public string? Destination { get; set; }
}