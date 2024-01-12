using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "buckets", "notifications", "list")]
public record GcloudStorageBucketsNotificationsListOptions(
[property: PositionalArgument] string Urls
) : GcloudOptions;