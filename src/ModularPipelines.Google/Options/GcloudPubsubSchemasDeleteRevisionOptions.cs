using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pubsub", "schemas", "delete-revision")]
public record GcloudPubsubSchemasDeleteRevisionOptions(
[property: PositionalArgument] string Schema
) : GcloudOptions;