using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks", "trustedaccess", "role", "list")]
public record AzAksTrustedaccessRoleListOptions(
[property: CommandSwitch("--location")] string Location
) : AzOptions;