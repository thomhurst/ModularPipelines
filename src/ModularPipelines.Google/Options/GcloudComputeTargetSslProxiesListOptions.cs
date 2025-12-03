using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "target-ssl-proxies", "list")]
public record GcloudComputeTargetSslProxiesListOptions : GcloudOptions;