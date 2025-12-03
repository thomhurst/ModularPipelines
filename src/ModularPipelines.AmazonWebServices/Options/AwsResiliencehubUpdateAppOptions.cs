using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resiliencehub", "update-app")]
public record AwsResiliencehubUpdateAppOptions(
[property: CliOption("--app-arn")] string AppArn
) : AwsOptions
{
    [CliOption("--assessment-schedule")]
    public string? AssessmentSchedule { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--event-subscriptions")]
    public string[]? EventSubscriptions { get; set; }

    [CliOption("--permission-model")]
    public string? PermissionModel { get; set; }

    [CliOption("--policy-arn")]
    public string? PolicyArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}