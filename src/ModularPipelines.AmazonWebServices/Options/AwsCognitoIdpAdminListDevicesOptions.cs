using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cognito-idp", "admin-list-devices")]
public record AwsCognitoIdpAdminListDevicesOptions(
[property: CommandSwitch("--user-pool-id")] string UserPoolId,
[property: CommandSwitch("--username")] string Username
) : AwsOptions
{
    [CommandSwitch("--limit")]
    public int? Limit { get; set; }

    [CommandSwitch("--pagination-token")]
    public string? PaginationToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}