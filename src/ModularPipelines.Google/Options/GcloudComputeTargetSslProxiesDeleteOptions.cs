using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "target-ssl-proxies", "delete")]
public record GcloudComputeTargetSslProxiesDeleteOptions(
[property: CliArgument] string Name
) : GcloudOptions;