using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pubsub", "schemas", "create")]
public record GcloudPubsubSchemasCreateOptions(
[property: PositionalArgument] string Schema,
[property: CommandSwitch("--type")] string Type,
[property: CommandSwitch("--definition")] string Definition,
[property: CommandSwitch("--definition-file")] string DefinitionFile
) : GcloudOptions;