using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pubsub", "schemas", "validate-message")]
public record GcloudPubsubSchemasValidateMessageOptions(
[property: CliOption("--message")] string Message,
[property: CliOption("--message-encoding")] string MessageEncoding,
[property: CliOption("--schema-name")] string SchemaName,
[property: CliOption("--type")] string Type,
[property: CliOption("--definition")] string Definition,
[property: CliOption("--definition-file")] string DefinitionFile
) : GcloudOptions;