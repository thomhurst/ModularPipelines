using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resiliencehub", "create-app")]
public record AwsResiliencehubCreateAppOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--assessment-schedule")]
    public string? AssessmentSchedule { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--event-subscriptions")]
    public string[]? EventSubscriptions { get; set; }

    [CliOption("--permission-model")]
    public string? PermissionModel { get; set; }

    [CliOption("--policy-arn")]
    public string? PolicyArn { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}