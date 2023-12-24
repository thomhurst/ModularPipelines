using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cognito-identity", "lookup-developer-identity")]
public record AwsCognitoIdentityLookupDeveloperIdentityOptions(
[property: CommandSwitch("--identity-pool-id")] string IdentityPoolId
) : AwsOptions
{
    [CommandSwitch("--identity-id")]
    public string? IdentityId { get; set; }

    [CommandSwitch("--developer-user-identifier")]
    public string? DeveloperUserIdentifier { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}