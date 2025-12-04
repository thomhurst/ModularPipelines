using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("spring", "storage", "add")]
public record AzSpringStorageAddOptions(
[property: CliOption("--account-key")] int AccountKey,
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service")] string Service,
[property: CliOption("--storage-type")] string StorageType
) : AzOptions;