using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pubsub", "schemas", "delete")]
public record GcloudPubsubSchemasDeleteOptions(
[property: CliArgument] string Schema
) : GcloudOptions;