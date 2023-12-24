using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resiliencehub", "update-app")]
public record AwsResiliencehubUpdateAppOptions(
[property: CommandSwitch("--app-arn")] string AppArn
) : AwsOptions
{
    [CommandSwitch("--assessment-schedule")]
    public string? AssessmentSchedule { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--event-subscriptions")]
    public string[]? EventSubscriptions { get; set; }

    [CommandSwitch("--permission-model")]
    public string? PermissionModel { get; set; }

    [CommandSwitch("--policy-arn")]
    public string? PolicyArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}