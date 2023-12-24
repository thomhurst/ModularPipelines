using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("customer-profiles", "create-integration-workflow")]
public record AwsCustomerProfilesCreateIntegrationWorkflowOptions(
[property: CommandSwitch("--domain-name")] string DomainName,
[property: CommandSwitch("--workflow-type")] string WorkflowType,
[property: CommandSwitch("--integration-config")] string IntegrationConfig,
[property: CommandSwitch("--object-type-name")] string ObjectTypeName,
[property: CommandSwitch("--role-arn")] string RoleArn
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}