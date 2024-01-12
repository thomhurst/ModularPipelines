using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pubsub", "schemas", "describe")]
public record GcloudPubsubSchemasDescribeOptions(
[property: PositionalArgument] string Schema
) : GcloudOptions;