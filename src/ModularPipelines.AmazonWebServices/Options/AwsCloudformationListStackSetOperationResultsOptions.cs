using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudformation", "list-stack-set-operation-results")]
public record AwsCloudformationListStackSetOperationResultsOptions(
[property: CommandSwitch("--stack-set-name")] string StackSetName,
[property: CommandSwitch("--operation-id")] string OperationId
) : AwsOptions
{
    [CommandSwitch("--call-as")]
    public string? CallAs { get; set; }

    [CommandSwitch("--filters")]
    public string[]? Filters { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}