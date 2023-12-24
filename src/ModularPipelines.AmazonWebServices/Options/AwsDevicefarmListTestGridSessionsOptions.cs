using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devicefarm", "list-test-grid-sessions")]
public record AwsDevicefarmListTestGridSessionsOptions(
[property: CommandSwitch("--project-arn")] string ProjectArn
) : AwsOptions
{
    [CommandSwitch("--status")]
    public string? Status { get; set; }

    [CommandSwitch("--creation-time-after")]
    public long? CreationTimeAfter { get; set; }

    [CommandSwitch("--creation-time-before")]
    public long? CreationTimeBefore { get; set; }

    [CommandSwitch("--end-time-after")]
    public long? EndTimeAfter { get; set; }

    [CommandSwitch("--end-time-before")]
    public long? EndTimeBefore { get; set; }

    [CommandSwitch("--max-result")]
    public int? MaxResult { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}