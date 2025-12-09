using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudformation", "list-stack-set-operation-results")]
public record AwsCloudformationListStackSetOperationResultsOptions(
[property: CliOption("--stack-set-name")] string StackSetName,
[property: CliOption("--operation-id")] string OperationId
) : AwsOptions
{
    [CliOption("--call-as")]
    public string? CallAs { get; set; }

    [CliOption("--filters")]
    public string[]? Filters { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}