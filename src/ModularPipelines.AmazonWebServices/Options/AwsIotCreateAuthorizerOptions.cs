using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "create-authorizer")]
public record AwsIotCreateAuthorizerOptions(
[property: CliOption("--authorizer-name")] string AuthorizerName,
[property: CliOption("--authorizer-function-arn")] string AuthorizerFunctionArn
) : AwsOptions
{
    [CliOption("--token-key-name")]
    public string? TokenKeyName { get; set; }

    [CliOption("--token-signing-public-keys")]
    public IEnumerable<KeyValue>? TokenSigningPublicKeys { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}