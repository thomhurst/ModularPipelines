using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("fis", "update-experiment-template")]
public record AwsFisUpdateExperimentTemplateOptions(
[property: CliOption("--id")] string Id
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--stop-conditions")]
    public string[]? StopConditions { get; set; }

    [CliOption("--targets")]
    public IEnumerable<KeyValue>? Targets { get; set; }

    [CliOption("--actions")]
    public IEnumerable<KeyValue>? Actions { get; set; }

    [CliOption("--role-arn")]
    public string? RoleArn { get; set; }

    [CliOption("--log-configuration")]
    public string? LogConfiguration { get; set; }

    [CliOption("--experiment-options")]
    public string? ExperimentOptions { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}