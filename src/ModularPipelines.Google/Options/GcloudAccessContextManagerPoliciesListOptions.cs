using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("access-context-manager", "policies", "list")]
public record GcloudAccessContextManagerPoliciesListOptions(
[property: CommandSwitch("--organization")] string Organization
) : GcloudOptions;