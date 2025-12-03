using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-idp", "initiate-auth")]
public record AwsCognitoIdpInitiateAuthOptions(
[property: CliOption("--auth-flow")] string AuthFlow,
[property: CliOption("--client-id")] string ClientId
) : AwsOptions
{
    [CliOption("--auth-parameters")]
    public IEnumerable<KeyValue>? AuthParameters { get; set; }

    [CliOption("--client-metadata")]
    public IEnumerable<KeyValue>? ClientMetadata { get; set; }

    [CliOption("--analytics-metadata")]
    public string? AnalyticsMetadata { get; set; }

    [CliOption("--user-context-data")]
    public string? UserContextData { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}