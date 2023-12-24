using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("customer-profiles", "get-workflow")]
public record AwsCustomerProfilesGetWorkflowOptions(
[property: CommandSwitch("--domain-name")] string DomainName,
[property: CommandSwitch("--workflow-id")] string WorkflowId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}