using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "buckets", "notifications", "delete")]
public record GcloudStorageBucketsNotificationsDeleteOptions(
[property: CliArgument] string Urls
) : GcloudOptions;