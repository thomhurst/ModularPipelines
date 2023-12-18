using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ad", "app", "permission", "add")]
public record AzAdAppPermissionAddOptions(
[property: CommandSwitch("--api")] string Api,
[property: CommandSwitch("--api-permissions")] string ApiPermissions,
[property: CommandSwitch("--id")] string Id
) : AzOptions;