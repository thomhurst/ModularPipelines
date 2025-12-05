using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("api-gateway", "api-configs", "create")]
public record GcloudApiGatewayApiConfigsCreateOptions(
[property: CliArgument] string ApiConfig,
[property: CliArgument] string Api,
[property: CliOption("--grpc-files")] string[] GrpcFiles,
[property: CliOption("--openapi-spec")] string[] OpenapiSpec
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--backend-auth-service-account")]
    public string? BackendAuthServiceAccount { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }
}