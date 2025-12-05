using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "target-ssl-proxies", "describe")]
public record GcloudComputeTargetSslProxiesDescribeOptions(
[property: CliArgument] string Name
) : GcloudOptions;