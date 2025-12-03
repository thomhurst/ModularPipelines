using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-identity", "set-identity-pool-roles")]
public record AwsCognitoIdentitySetIdentityPoolRolesOptions(
[property: CliOption("--identity-pool-id")] string IdentityPoolId,
[property: CliOption("--roles")] IEnumerable<KeyValue> Roles
) : AwsOptions
{
    [CliOption("--role-mappings")]
    public IEnumerable<KeyValue>? RoleMappings { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}