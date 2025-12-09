using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-idp", "admin-initiate-auth")]
public record AwsCognitoIdpAdminInitiateAuthOptions(
[property: CliOption("--user-pool-id")] string UserPoolId,
[property: CliOption("--client-id")] string ClientId,
[property: CliOption("--auth-flow")] string AuthFlow
) : AwsOptions
{
    [CliOption("--auth-parameters")]
    public IEnumerable<KeyValue>? AuthParameters { get; set; }

    [CliOption("--client-metadata")]
    public IEnumerable<KeyValue>? ClientMetadata { get; set; }

    [CliOption("--analytics-metadata")]
    public string? AnalyticsMetadata { get; set; }

    [CliOption("--context-data")]
    public string? ContextData { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}