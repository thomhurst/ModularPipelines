using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appsync", "create-graphql-api")]
public record AwsAppsyncCreateGraphqlApiOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--authentication-type")] string AuthenticationType
) : AwsOptions
{
    [CliOption("--log-config")]
    public string? LogConfig { get; set; }

    [CliOption("--user-pool-config")]
    public string? UserPoolConfig { get; set; }

    [CliOption("--open-id-connect-config")]
    public string? OpenIdConnectConfig { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--additional-authentication-providers")]
    public string[]? AdditionalAuthenticationProviders { get; set; }

    [CliOption("--lambda-authorizer-config")]
    public string? LambdaAuthorizerConfig { get; set; }

    [CliOption("--visibility")]
    public string? Visibility { get; set; }

    [CliOption("--api-type")]
    public string? ApiType { get; set; }

    [CliOption("--merged-api-execution-role-arn")]
    public string? MergedApiExecutionRoleArn { get; set; }

    [CliOption("--owner-contact")]
    public string? OwnerContact { get; set; }

    [CliOption("--introspection-config")]
    public string? IntrospectionConfig { get; set; }

    [CliOption("--query-depth-limit")]
    public int? QueryDepthLimit { get; set; }

    [CliOption("--resolver-count-limit")]
    public int? ResolverCountLimit { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}