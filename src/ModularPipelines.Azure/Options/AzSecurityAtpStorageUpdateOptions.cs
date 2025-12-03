using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("security", "atp", "storage", "update")]
public record AzSecurityAtpStorageUpdateOptions(
[property: CliFlag("--is-enabled")] bool IsEnabled,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--storage-account")] int StorageAccount
) : AzOptions;