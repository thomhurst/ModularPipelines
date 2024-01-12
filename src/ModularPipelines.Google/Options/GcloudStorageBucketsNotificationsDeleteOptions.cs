using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "buckets", "notifications", "delete")]
public record GcloudStorageBucketsNotificationsDeleteOptions(
[property: PositionalArgument] string Urls
) : GcloudOptions;