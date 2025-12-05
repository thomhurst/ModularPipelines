using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("spanner", "instances", "delete")]
public record GcloudSpannerInstancesDeleteOptions(
[property: CliArgument] string Instance
) : GcloudOptions;