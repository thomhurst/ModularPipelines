using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("csvmware", "resource-pool", "show")]
public record AzCsvmwareResourcePoolShowOptions(
[property: CliOption("--location")] string Location,
[property: CliOption("--name")] string Name,
[property: CliOption("--private-cloud")] string PrivateCloud
) : AzOptions;