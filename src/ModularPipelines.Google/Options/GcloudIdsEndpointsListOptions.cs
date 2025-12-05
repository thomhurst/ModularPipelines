using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ids", "endpoints", "list")]
public record GcloudIdsEndpointsListOptions : GcloudOptions;