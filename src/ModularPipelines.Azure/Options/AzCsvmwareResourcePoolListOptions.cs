using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("csvmware", "resource-pool", "list")]
public record AzCsvmwareResourcePoolListOptions(
[property: CliOption("--location")] string Location,
[property: CliOption("--private-cloud")] string PrivateCloud
) : AzOptions;