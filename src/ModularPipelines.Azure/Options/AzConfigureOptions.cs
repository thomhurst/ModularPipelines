using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("configure")]
public record AzConfigureOptions(
[property: CommandSwitch("--uri")] string Uri
) : AzOptions
{
    [CommandSwitch("--defaults")]
    public string? Defaults { get; set; }

    [BooleanCommandSwitch("--list-defaults")]
    public bool? ListDefaults { get; set; }

    [CommandSwitch("--scope")]
    public string? Scope { get; set; }
}