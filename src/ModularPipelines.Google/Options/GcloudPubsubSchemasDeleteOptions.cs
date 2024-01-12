using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pubsub", "schemas", "delete")]
public record GcloudPubsubSchemasDeleteOptions(
[property: PositionalArgument] string Schema
) : GcloudOptions;