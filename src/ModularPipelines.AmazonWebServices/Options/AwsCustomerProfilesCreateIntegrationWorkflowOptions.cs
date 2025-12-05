using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("customer-profiles", "create-integration-workflow")]
public record AwsCustomerProfilesCreateIntegrationWorkflowOptions(
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--workflow-type")] string WorkflowType,
[property: CliOption("--integration-config")] string IntegrationConfig,
[property: CliOption("--object-type-name")] string ObjectTypeName,
[property: CliOption("--role-arn")] string RoleArn
) : AwsOptions
{
    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}