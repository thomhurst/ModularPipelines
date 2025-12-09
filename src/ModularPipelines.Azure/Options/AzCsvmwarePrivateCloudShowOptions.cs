using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("csvmware", "private-cloud", "show")]
public record AzCsvmwarePrivateCloudShowOptions(
[property: CliOption("--location")] string Location,
[property: CliOption("--name")] string Name
) : AzOptions;