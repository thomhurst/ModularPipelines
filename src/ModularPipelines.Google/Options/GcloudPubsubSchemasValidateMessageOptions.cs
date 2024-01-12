using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pubsub", "schemas", "validate-message")]
public record GcloudPubsubSchemasValidateMessageOptions(
[property: CommandSwitch("--message")] string Message,
[property: CommandSwitch("--message-encoding")] string MessageEncoding,
[property: CommandSwitch("--schema-name")] string SchemaName,
[property: CommandSwitch("--type")] string Type,
[property: CommandSwitch("--definition")] string Definition,
[property: CommandSwitch("--definition-file")] string DefinitionFile
) : GcloudOptions;