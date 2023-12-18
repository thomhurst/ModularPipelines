using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ad", "app", "permission", "list-grants")]
public record AzAdAppPermissionListGrantsOptions : AzOptions
{
    [CommandSwitch("--filter")]
    public string? Filter { get; set; }

    [CommandSwitch("--id")]
    public string? Id { get; set; }

    [BooleanCommandSwitch("--show-resource-name")]
    public bool? ShowResourceName { get; set; }
}