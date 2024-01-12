using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("access-context-manager", "policies", "update")]
public record GcloudAccessContextManagerPoliciesUpdateOptions(
[property: PositionalArgument] string Policy
) : GcloudOptions
{
    [CommandSwitch("--title")]
    public string? Title { get; set; }
}