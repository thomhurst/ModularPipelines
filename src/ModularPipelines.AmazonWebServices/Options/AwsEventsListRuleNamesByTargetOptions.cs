using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("events", "list-rule-names-by-target")]
public record AwsEventsListRuleNamesByTargetOptions(
[property: CliOption("--target-arn")] string TargetArn
) : AwsOptions
{
    [CliOption("--event-bus-name")]
    public string? EventBusName { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}