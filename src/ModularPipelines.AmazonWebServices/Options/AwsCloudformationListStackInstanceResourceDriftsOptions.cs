using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudformation", "list-stack-instance-resource-drifts")]
public record AwsCloudformationListStackInstanceResourceDriftsOptions(
[property: CliOption("--stack-set-name")] string StackSetName,
[property: CliOption("--stack-instance-account")] string StackInstanceAccount,
[property: CliOption("--stack-instance-region")] string StackInstanceRegion,
[property: CliOption("--operation-id")] string OperationId
) : AwsOptions
{
    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--stack-instance-resource-drift-statuses")]
    public string[]? StackInstanceResourceDriftStatuses { get; set; }

    [CliOption("--call-as")]
    public string? CallAs { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}