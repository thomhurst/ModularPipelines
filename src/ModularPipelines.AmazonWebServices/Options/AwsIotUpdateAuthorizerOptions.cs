using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "update-authorizer")]
public record AwsIotUpdateAuthorizerOptions(
[property: CommandSwitch("--authorizer-name")] string AuthorizerName
) : AwsOptions
{
    [CommandSwitch("--authorizer-function-arn")]
    public string? AuthorizerFunctionArn { get; set; }

    [CommandSwitch("--token-key-name")]
    public string? TokenKeyName { get; set; }

    [CommandSwitch("--token-signing-public-keys")]
    public IEnumerable<KeyValue>? TokenSigningPublicKeys { get; set; }

    [CommandSwitch("--status")]
    public string? Status { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}