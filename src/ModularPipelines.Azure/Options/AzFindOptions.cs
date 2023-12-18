using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("find")]
public record AzFindOptions(
[property: CommandSwitch("--uri")] string Uri
) : AzOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? CliTerm { get; set; }
}