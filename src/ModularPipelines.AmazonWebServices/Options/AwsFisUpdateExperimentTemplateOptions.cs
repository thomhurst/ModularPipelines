using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("fis", "update-experiment-template")]
public record AwsFisUpdateExperimentTemplateOptions(
[property: CommandSwitch("--id")] string Id
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--stop-conditions")]
    public string[]? StopConditions { get; set; }

    [CommandSwitch("--targets")]
    public IEnumerable<KeyValue>? Targets { get; set; }

    [CommandSwitch("--actions")]
    public IEnumerable<KeyValue>? Actions { get; set; }

    [CommandSwitch("--role-arn")]
    public string? RoleArn { get; set; }

    [CommandSwitch("--log-configuration")]
    public string? LogConfiguration { get; set; }

    [CommandSwitch("--experiment-options")]
    public string? ExperimentOptions { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}