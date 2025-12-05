using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("spanner", "samples", "list")]
public record GcloudSpannerSamplesListOptions : GcloudOptions;