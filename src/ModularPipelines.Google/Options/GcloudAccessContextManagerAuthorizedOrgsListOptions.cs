using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("access-context-manager", "authorized-orgs", "list")]
public record GcloudAccessContextManagerAuthorizedOrgsListOptions : GcloudOptions
{
    [CommandSwitch("--policy")]
    public string? Policy { get; set; }
}