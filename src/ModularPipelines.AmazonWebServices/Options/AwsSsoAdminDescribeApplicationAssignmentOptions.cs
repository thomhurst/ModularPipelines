using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sso-admin", "describe-application-assignment")]
public record AwsSsoAdminDescribeApplicationAssignmentOptions(
[property: CommandSwitch("--application-arn")] string ApplicationArn,
[property: CommandSwitch("--principal-id")] string PrincipalId,
[property: CommandSwitch("--principal-type")] string PrincipalType
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}