using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cognito-idp", "get-group")]
public record AwsCognitoIdpGetGroupOptions(
[property: CommandSwitch("--group-name")] string GroupName,
[property: CommandSwitch("--user-pool-id")] string UserPoolId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}