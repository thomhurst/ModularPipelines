using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pubsub", "lite-topics", "delete")]
public record GcloudPubsubLiteTopicsDeleteOptions(
[property: CliArgument] string Topic,
[property: CliArgument] string Location
) : GcloudOptions;