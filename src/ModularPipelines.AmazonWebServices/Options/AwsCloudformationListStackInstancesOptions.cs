using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudformation", "list-stack-instances")]
public record AwsCloudformationListStackInstancesOptions(
[property: CommandSwitch("--stack-set-name")] string StackSetName
) : AwsOptions
{
    [CommandSwitch("--filters")]
    public string[]? Filters { get; set; }

    [CommandSwitch("--stack-instance-account")]
    public string? StackInstanceAccount { get; set; }

    [CommandSwitch("--stack-instance-region")]
    public string? StackInstanceRegion { get; set; }

    [CommandSwitch("--call-as")]
    public string? CallAs { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}