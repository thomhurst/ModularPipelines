using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datastore", "operations", "describe")]
public record GcloudDatastoreOperationsDescribeOptions(
[property: CliArgument] string Name
) : GcloudOptions;