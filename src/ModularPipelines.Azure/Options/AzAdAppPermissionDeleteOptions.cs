using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ad", "app", "permission", "delete")]
public record AzAdAppPermissionDeleteOptions(
[property: CliOption("--api")] string Api,
[property: CliOption("--id")] string Id
) : AzOptions
{
    [CliOption("--api-permissions")]
    public string? ApiPermissions { get; set; }
}