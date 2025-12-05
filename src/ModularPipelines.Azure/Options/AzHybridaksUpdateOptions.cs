using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("hybridaks", "update")]
public record AzHybridaksUpdateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--aad-admin-group-object-ids")]
    public string? AadAdminGroupObjectIds { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}