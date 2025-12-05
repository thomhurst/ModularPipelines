using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("customer-profiles", "delete-workflow")]
public record AwsCustomerProfilesDeleteWorkflowOptions(
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--workflow-id")] string WorkflowId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}