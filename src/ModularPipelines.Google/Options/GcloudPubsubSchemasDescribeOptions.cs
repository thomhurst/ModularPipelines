using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pubsub", "schemas", "describe")]
public record GcloudPubsubSchemasDescribeOptions(
[property: CliArgument] string Schema
) : GcloudOptions;