using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "buckets", "notifications", "list")]
public record GcloudStorageBucketsNotificationsListOptions(
[property: CliArgument] string Urls
) : GcloudOptions;