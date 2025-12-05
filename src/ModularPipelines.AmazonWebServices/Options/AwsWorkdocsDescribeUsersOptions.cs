using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workdocs", "describe-users")]
public record AwsWorkdocsDescribeUsersOptions : AwsOptions
{
    [CliOption("--authentication-token")]
    public string? AuthenticationToken { get; set; }

    [CliOption("--organization-id")]
    public string? OrganizationId { get; set; }

    [CliOption("--user-ids")]
    public string? UserIds { get; set; }

    [CliOption("--include")]
    public string? Include { get; set; }

    [CliOption("--order")]
    public string? Order { get; set; }

    [CliOption("--sort")]
    public string? Sort { get; set; }

    [CliOption("--fields")]
    public string? Fields { get; set; }

    [CliOption("--user-query")]
    public string? UserQuery { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}