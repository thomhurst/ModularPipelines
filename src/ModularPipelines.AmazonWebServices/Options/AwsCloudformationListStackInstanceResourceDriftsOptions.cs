using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudformation", "list-stack-instance-resource-drifts")]
public record AwsCloudformationListStackInstanceResourceDriftsOptions(
[property: CommandSwitch("--stack-set-name")] string StackSetName,
[property: CommandSwitch("--stack-instance-account")] string StackInstanceAccount,
[property: CommandSwitch("--stack-instance-region")] string StackInstanceRegion,
[property: CommandSwitch("--operation-id")] string OperationId
) : AwsOptions
{
    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--stack-instance-resource-drift-statuses")]
    public string[]? StackInstanceResourceDriftStatuses { get; set; }

    [CommandSwitch("--call-as")]
    public string? CallAs { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}