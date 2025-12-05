using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("events", "put-rule")]
public record AwsEventsPutRuleOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--schedule-expression")]
    public string? ScheduleExpression { get; set; }

    [CliOption("--event-pattern")]
    public string? EventPattern { get; set; }

    [CliOption("--state")]
    public string? State { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--role-arn")]
    public string? RoleArn { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--event-bus-name")]
    public string? EventBusName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}