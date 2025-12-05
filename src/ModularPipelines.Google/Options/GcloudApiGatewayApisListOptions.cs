using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("api-gateway", "apis", "list")]
public record GcloudApiGatewayApisListOptions : GcloudOptions;