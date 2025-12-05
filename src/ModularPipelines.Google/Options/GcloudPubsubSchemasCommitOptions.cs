using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pubsub", "schemas", "commit")]
public record GcloudPubsubSchemasCommitOptions(
[property: CliArgument] string Schema,
[property: CliOption("--type")] string Type,
[property: CliOption("--definition")] string Definition,
[property: CliOption("--definition-file")] string DefinitionFile
) : GcloudOptions;