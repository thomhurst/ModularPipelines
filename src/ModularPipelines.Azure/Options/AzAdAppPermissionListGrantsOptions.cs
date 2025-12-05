using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ad", "app", "permission", "list-grants")]
public record AzAdAppPermissionListGrantsOptions : AzOptions
{
    [CliOption("--filter")]
    public string? Filter { get; set; }

    [CliOption("--id")]
    public string? Id { get; set; }

    [CliFlag("--show-resource-name")]
    public bool? ShowResourceName { get; set; }
}