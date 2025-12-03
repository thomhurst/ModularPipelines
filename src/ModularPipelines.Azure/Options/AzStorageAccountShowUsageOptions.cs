using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "account", "show-usage")]
public record AzStorageAccountShowUsageOptions(
[property: CliOption("--location")] string Location
) : AzOptions;