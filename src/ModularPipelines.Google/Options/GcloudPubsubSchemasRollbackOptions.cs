using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pubsub", "schemas", "rollback")]
public record GcloudPubsubSchemasRollbackOptions(
[property: CliArgument] string Schema,
[property: CliOption("--revision-id")] string RevisionId
) : GcloudOptions;