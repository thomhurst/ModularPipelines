using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sso-admin", "list-account-assignments-for-principal")]
public record AwsSsoAdminListAccountAssignmentsForPrincipalOptions(
[property: CliOption("--instance-arn")] string InstanceArn,
[property: CliOption("--principal-id")] string PrincipalId,
[property: CliOption("--principal-type")] string PrincipalType
) : AwsOptions
{
    [CliOption("--filter")]
    public string? Filter { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}