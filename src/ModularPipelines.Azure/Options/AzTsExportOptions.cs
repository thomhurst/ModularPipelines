using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ts", "export")]
public record AzTsExportOptions(
[property: CommandSwitch("--output-folder")] string OutputFolder
) : AzOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--template-spec")]
    public string? TemplateSpec { get; set; }

    [CommandSwitch("--version")]
    public string? Version { get; set; }
}