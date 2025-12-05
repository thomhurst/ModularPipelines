using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("apim", "deletedservice", "purge")]
public record AzApimDeletedservicePurgeOptions(
[property: CliOption("--location")] string Location,
[property: CliOption("--service-name")] string ServiceName
) : AzOptions;