using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("api-gateway", "api-configs", "create")]
public record GcloudApiGatewayApiConfigsCreateOptions(
[property: PositionalArgument] string ApiConfig,
[property: PositionalArgument] string Api,
[property: CommandSwitch("--grpc-files")] string[] GrpcFiles,
[property: CommandSwitch("--openapi-spec")] string[] OpenapiSpec
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--backend-auth-service-account")]
    public string? BackendAuthServiceAccount { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }
}