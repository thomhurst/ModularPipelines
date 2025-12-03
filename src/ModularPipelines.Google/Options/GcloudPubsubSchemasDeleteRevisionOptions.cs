using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pubsub", "schemas", "delete-revision")]
public record GcloudPubsubSchemasDeleteRevisionOptions(
[property: CliArgument] string Schema
) : GcloudOptions;