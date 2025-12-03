using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("firestore", "indexes", "composite", "describe")]
public record GcloudFirestoreIndexesCompositeDescribeOptions(
[property: CliArgument] string Index,
[property: CliArgument] string Database
) : GcloudOptions;