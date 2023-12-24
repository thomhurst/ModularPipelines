using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("organizations", "describe-create-account-status")]
public record AwsOrganizationsDescribeCreateAccountStatusOptions(
[property: CommandSwitch("--create-account-request-id")] string CreateAccountRequestId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}