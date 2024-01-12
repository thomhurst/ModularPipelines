using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("firestore", "indexes", "composite", "describe")]
public record GcloudFirestoreIndexesCompositeDescribeOptions(
[property: PositionalArgument] string Index,
[property: PositionalArgument] string Database
) : GcloudOptions;