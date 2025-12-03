using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apim", "deletedservice", "show")]
public record AzApimDeletedserviceShowOptions(
[property: CliOption("--location")] string Location,
[property: CliOption("--service-name")] string ServiceName
) : AzOptions;