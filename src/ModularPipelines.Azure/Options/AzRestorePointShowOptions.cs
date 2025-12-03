using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("restore-point", "show")]
public record AzRestorePointShowOptions(
[property: CliOption("--collection-name")] string CollectionName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--instance-view")]
    public string? InstanceView { get; set; }
}