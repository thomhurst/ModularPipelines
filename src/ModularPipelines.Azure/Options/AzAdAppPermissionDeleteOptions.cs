using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ad", "app", "permission", "delete")]
public record AzAdAppPermissionDeleteOptions(
[property: CommandSwitch("--api")] string Api,
[property: CommandSwitch("--id")] string Id
) : AzOptions
{
    [CommandSwitch("--api-permissions")]
    public string? ApiPermissions { get; set; }
}