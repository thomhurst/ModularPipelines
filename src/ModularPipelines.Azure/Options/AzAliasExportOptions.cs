using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("alias", "export")]
public record AzAliasExportOptions(
[property: CommandSwitch("--source")] string Source
) : AzOptions
{
    [CommandSwitch("--exclude")]
    public string? Exclude { get; set; }

    [CommandSwitch("--path")]
    public string? Path { get; set; }
}

