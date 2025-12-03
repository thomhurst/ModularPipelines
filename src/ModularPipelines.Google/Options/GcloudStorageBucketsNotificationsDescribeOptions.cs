using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "buckets", "notifications", "describe")]
public record GcloudStorageBucketsNotificationsDescribeOptions(
[property: CliArgument] string Url
) : GcloudOptions;