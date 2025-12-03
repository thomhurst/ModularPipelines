using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pubsub", "lite-topics", "describe")]
public record GcloudPubsubLiteTopicsDescribeOptions(
[property: CliArgument] string Topic,
[property: CliArgument] string Location
) : GcloudOptions;