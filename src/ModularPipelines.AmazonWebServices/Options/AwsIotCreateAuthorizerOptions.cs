using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "create-authorizer")]
public record AwsIotCreateAuthorizerOptions(
[property: CommandSwitch("--authorizer-name")] string AuthorizerName,
[property: CommandSwitch("--authorizer-function-arn")] string AuthorizerFunctionArn
) : AwsOptions
{
    [CommandSwitch("--token-key-name")]
    public string? TokenKeyName { get; set; }

    [CommandSwitch("--token-signing-public-keys")]
    public IEnumerable<KeyValue>? TokenSigningPublicKeys { get; set; }

    [CommandSwitch("--status")]
    public string? Status { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}