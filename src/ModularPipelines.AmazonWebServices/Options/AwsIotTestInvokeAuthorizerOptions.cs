using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "test-invoke-authorizer")]
public record AwsIotTestInvokeAuthorizerOptions(
[property: CommandSwitch("--authorizer-name")] string AuthorizerName
) : AwsOptions
{
    [CommandSwitch("--token")]
    public string? Token { get; set; }

    [CommandSwitch("--token-signature")]
    public string? TokenSignature { get; set; }

    [CommandSwitch("--http-context")]
    public string? HttpContext { get; set; }

    [CommandSwitch("--mqtt-context")]
    public string? MqttContext { get; set; }

    [CommandSwitch("--tls-context")]
    public string? TlsContext { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}