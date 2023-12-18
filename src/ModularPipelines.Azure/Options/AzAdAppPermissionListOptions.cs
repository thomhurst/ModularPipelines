using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ad", "app", "permission", "list")]
public record AzAdAppPermissionListOptions(
[property: CommandSwitch("--id")] string Id
) : AzOptions;