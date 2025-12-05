using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-sync", "set-cognito-events")]
public record AwsCognitoSyncSetCognitoEventsOptions(
[property: CliOption("--identity-pool-id")] string IdentityPoolId,
[property: CliOption("--events")] IEnumerable<KeyValue> Events
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}