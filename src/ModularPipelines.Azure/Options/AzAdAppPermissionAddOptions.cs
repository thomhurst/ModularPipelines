using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ad", "app", "permission", "add")]
public record AzAdAppPermissionAddOptions(
[property: CommandSwitch("--api")] string Api,
[property: CommandSwitch("--api-permissions")] string ApiPermissions,
[property: CommandSwitch("--id")] string Id
) : AzOptions
{
}