using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sso-admin", "delete-application-assignment")]
public record AwsSsoAdminDeleteApplicationAssignmentOptions(
[property: CliOption("--application-arn")] string ApplicationArn,
[property: CliOption("--principal-id")] string PrincipalId,
[property: CliOption("--principal-type")] string PrincipalType
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}