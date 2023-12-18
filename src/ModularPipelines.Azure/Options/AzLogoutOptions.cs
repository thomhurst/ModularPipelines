using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("logout")]
public record AzLogoutOptions : AzOptions
{
    [CommandSwitch("--username")]
    public string? Username { get; set; }
}