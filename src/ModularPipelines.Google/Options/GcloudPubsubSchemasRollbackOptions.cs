using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pubsub", "schemas", "rollback")]
public record GcloudPubsubSchemasRollbackOptions(
[property: PositionalArgument] string Schema,
[property: CommandSwitch("--revision-id")] string RevisionId
) : GcloudOptions;