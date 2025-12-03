using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("csvmware", "private-cloud", "list")]
public record AzCsvmwarePrivateCloudListOptions(
[property: CliOption("--location")] string Location
) : AzOptions;