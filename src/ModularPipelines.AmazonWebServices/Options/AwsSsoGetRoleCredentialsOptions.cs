using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sso", "get-role-credentials")]
public record AwsSsoGetRoleCredentialsOptions(
[property: CommandSwitch("--role-name")] string RoleName,
[property: CommandSwitch("--account-id")] string AccountId,
[property: CommandSwitch("--access-token")] string AccessToken
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}