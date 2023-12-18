using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("logout")]
public record AzLogoutOptions(
[property: CommandSwitch("--uri")] string Uri
) : AzOptions
{
    [CommandSwitch("--username")]
    public string? Username { get; set; }
}