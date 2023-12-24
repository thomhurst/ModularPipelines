using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appsync", "update-graphql-api")]
public record AwsAppsyncUpdateGraphqlApiOptions(
[property: CommandSwitch("--api-id")] string ApiId,
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--log-config")]
    public string? LogConfig { get; set; }

    [CommandSwitch("--authentication-type")]
    public string? AuthenticationType { get; set; }

    [CommandSwitch("--user-pool-config")]
    public string? UserPoolConfig { get; set; }

    [CommandSwitch("--open-id-connect-config")]
    public string? OpenIdConnectConfig { get; set; }

    [CommandSwitch("--additional-authentication-providers")]
    public string[]? AdditionalAuthenticationProviders { get; set; }

    [CommandSwitch("--lambda-authorizer-config")]
    public string? LambdaAuthorizerConfig { get; set; }

    [CommandSwitch("--merged-api-execution-role-arn")]
    public string? MergedApiExecutionRoleArn { get; set; }

    [CommandSwitch("--owner-contact")]
    public string? OwnerContact { get; set; }

    [CommandSwitch("--introspection-config")]
    public string? IntrospectionConfig { get; set; }

    [CommandSwitch("--query-depth-limit")]
    public int? QueryDepthLimit { get; set; }

    [CommandSwitch("--resolver-count-limit")]
    public int? ResolverCountLimit { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}