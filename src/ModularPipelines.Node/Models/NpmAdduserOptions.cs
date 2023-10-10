using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("adduser")]
public record NpmAdduserOptions : NpmOptions
{
    [CommandSwitch("--registry")]
    public Uri? Registry { get; set; }

    [CommandSwitch("--scope")]
    public string? Scope { get; set; }

    [CommandSwitch("--auth-type")]
    public string? AuthType { get; set; }

}