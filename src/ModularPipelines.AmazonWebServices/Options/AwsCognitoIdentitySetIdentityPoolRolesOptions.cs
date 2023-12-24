using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cognito-identity", "set-identity-pool-roles")]
public record AwsCognitoIdentitySetIdentityPoolRolesOptions(
[property: CommandSwitch("--identity-pool-id")] string IdentityPoolId,
[property: CommandSwitch("--roles")] IEnumerable<KeyValue> Roles
) : AwsOptions
{
    [CommandSwitch("--role-mappings")]
    public IEnumerable<KeyValue>? RoleMappings { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}