using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("spanner", "instances", "describe")]
public record GcloudSpannerInstancesDescribeOptions(
[property: CliArgument] string Instance
) : GcloudOptions;