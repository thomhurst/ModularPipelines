using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workdocs", "search-resources")]
public record AwsWorkdocsSearchResourcesOptions : AwsOptions
{
    [CliOption("--authentication-token")]
    public string? AuthenticationToken { get; set; }

    [CliOption("--query-text")]
    public string? QueryText { get; set; }

    [CliOption("--query-scopes")]
    public string[]? QueryScopes { get; set; }

    [CliOption("--organization-id")]
    public string? OrganizationId { get; set; }

    [CliOption("--additional-response-fields")]
    public string[]? AdditionalResponseFields { get; set; }

    [CliOption("--filters")]
    public string? Filters { get; set; }

    [CliOption("--order-by")]
    public string[]? OrderBy { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}