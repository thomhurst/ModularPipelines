using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "update-authorizer")]
public record AwsIotUpdateAuthorizerOptions(
[property: CliOption("--authorizer-name")] string AuthorizerName
) : AwsOptions
{
    [CliOption("--authorizer-function-arn")]
    public string? AuthorizerFunctionArn { get; set; }

    [CliOption("--token-key-name")]
    public string? TokenKeyName { get; set; }

    [CliOption("--token-signing-public-keys")]
    public IEnumerable<KeyValue>? TokenSigningPublicKeys { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}