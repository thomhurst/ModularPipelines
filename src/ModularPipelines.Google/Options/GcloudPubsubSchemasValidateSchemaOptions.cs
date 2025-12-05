using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pubsub", "schemas", "validate-schema")]
public record GcloudPubsubSchemasValidateSchemaOptions(
[property: CliOption("--type")] string Type,
[property: CliOption("--definition")] string Definition,
[property: CliOption("--definition-file")] string DefinitionFile
) : GcloudOptions;