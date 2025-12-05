using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("customer-profiles", "list-workflows")]
public record AwsCustomerProfilesListWorkflowsOptions(
[property: CliOption("--domain-name")] string DomainName
) : AwsOptions
{
    [CliOption("--workflow-type")]
    public string? WorkflowType { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--query-start-date")]
    public long? QueryStartDate { get; set; }

    [CliOption("--query-end-date")]
    public long? QueryEndDate { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}