using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("spanner", "instance-configs", "list")]
public record GcloudSpannerInstanceConfigsListOptions : GcloudOptions;