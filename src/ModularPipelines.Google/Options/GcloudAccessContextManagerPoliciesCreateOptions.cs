using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("access-context-manager", "policies", "create")]
public record GcloudAccessContextManagerPoliciesCreateOptions(
[property: CommandSwitch("--organization")] string Organization,
[property: CommandSwitch("--title")] string Title
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--scopes")]
    public string[]? Scopes { get; set; }
}