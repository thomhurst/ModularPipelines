using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("containerapp", "env", "storage", "remove")]
public record AzContainerappEnvStorageRemoveOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--storage-name")] string StorageName
) : AzOptions
{
    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}