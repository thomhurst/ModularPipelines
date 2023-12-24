using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cognito-idp", "list-devices")]
public record AwsCognitoIdpListDevicesOptions(
[property: CommandSwitch("--access-token")] string AccessToken
) : AwsOptions
{
    [CommandSwitch("--limit")]
    public int? Limit { get; set; }

    [CommandSwitch("--pagination-token")]
    public string? PaginationToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}