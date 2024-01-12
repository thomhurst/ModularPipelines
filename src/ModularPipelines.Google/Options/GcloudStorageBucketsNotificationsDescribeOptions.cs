using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "buckets", "notifications", "describe")]
public record GcloudStorageBucketsNotificationsDescribeOptions(
[property: PositionalArgument] string Url
) : GcloudOptions;