using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "target-ssl-proxies", "list")]
public record GcloudComputeTargetSslProxiesListOptions : GcloudOptions;