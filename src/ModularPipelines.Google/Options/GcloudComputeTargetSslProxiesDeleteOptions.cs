using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "target-ssl-proxies", "delete")]
public record GcloudComputeTargetSslProxiesDeleteOptions(
[property: PositionalArgument] string Name
) : GcloudOptions;