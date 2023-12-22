using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("provider", "permission", "list")]
public record AzProviderPermissionListOptions(
[property: CommandSwitch("--namespace")] string Namespace
) : AzOptions;