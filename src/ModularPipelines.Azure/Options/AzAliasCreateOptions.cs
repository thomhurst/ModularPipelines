using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("alias", "create")]
public record AzAliasCreateOptions(
[property: CommandSwitch("--command")] string Command,
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--exclude")]
    public string? Exclude { get; set; }

    [CommandSwitch("--path")]
    public string? Path { get; set; }
}