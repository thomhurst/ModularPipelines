using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("s3control", "list-access-grants")]
public record AwsS3controlListAccessGrantsOptions(
[property: CommandSwitch("--account-id")] string AccountId
) : AwsOptions
{
    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--grantee-type")]
    public string? GranteeType { get; set; }

    [CommandSwitch("--grantee-identifier")]
    public string? GranteeIdentifier { get; set; }

    [CommandSwitch("--permission")]
    public string? Permission { get; set; }

    [CommandSwitch("--grant-scope")]
    public string? GrantScope { get; set; }

    [CommandSwitch("--application-arn")]
    public string? ApplicationArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}