using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("security", "atp", "storage", "show")]
public record AzSecurityAtpStorageShowOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--storage-account")] int StorageAccount
) : AzOptions;