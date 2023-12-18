using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acr", "repository", "update")]
public record AzAcrRepositoryUpdateOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [BooleanCommandSwitch("--delete-enabled")]
    public bool? DeleteEnabled { get; set; }

    [CommandSwitch("--image")]
    public string? Image { get; set; }

    [BooleanCommandSwitch("--list-enabled")]
    public bool? ListEnabled { get; set; }

    [CommandSwitch("--password")]
    public string? Password { get; set; }

    [BooleanCommandSwitch("--read-enabled")]
    public bool? ReadEnabled { get; set; }

    [CommandSwitch("--repository")]
    public string? Repository { get; set; }

    [CommandSwitch("--suffix")]
    public string? Suffix { get; set; }

    [CommandSwitch("--username")]
    public string? Username { get; set; }

    [BooleanCommandSwitch("--write-enabled")]
    public bool? WriteEnabled { get; set; }
}