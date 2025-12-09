using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appsync", "update-graphql-api")]
public record AwsAppsyncUpdateGraphqlApiOptions(
[property: CliOption("--api-id")] string ApiId,
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--log-config")]
    public string? LogConfig { get; set; }

    [CliOption("--authentication-type")]
    public string? AuthenticationType { get; set; }

    [CliOption("--user-pool-config")]
    public string? UserPoolConfig { get; set; }

    [CliOption("--open-id-connect-config")]
    public string? OpenIdConnectConfig { get; set; }

    [CliOption("--additional-authentication-providers")]
    public string[]? AdditionalAuthenticationProviders { get; set; }

    [CliOption("--lambda-authorizer-config")]
    public string? LambdaAuthorizerConfig { get; set; }

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