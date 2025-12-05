using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "test-invoke-authorizer")]
public record AwsIotTestInvokeAuthorizerOptions(
[property: CliOption("--authorizer-name")] string AuthorizerName
) : AwsOptions
{
    [CliOption("--token")]
    public string? Token { get; set; }

    [CliOption("--token-signature")]
    public string? TokenSignature { get; set; }

    [CliOption("--http-context")]
    public string? HttpContext { get; set; }

    [CliOption("--mqtt-context")]
    public string? MqttContext { get; set; }

    [CliOption("--tls-context")]
    public string? TlsContext { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}