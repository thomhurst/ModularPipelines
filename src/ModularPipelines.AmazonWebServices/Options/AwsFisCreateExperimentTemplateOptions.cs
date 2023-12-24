using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("fis", "create-experiment-template")]
public record AwsFisCreateExperimentTemplateOptions(
[property: CommandSwitch("--description")] string Description,
[property: CommandSwitch("--stop-conditions")] string[] StopConditions,
[property: CommandSwitch("--actions")] IEnumerable<KeyValue> Actions,
[property: CommandSwitch("--role-arn")] string RoleArn
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--targets")]
    public IEnumerable<KeyValue>? Targets { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--log-configuration")]
    public string? LogConfiguration { get; set; }

    [CommandSwitch("--experiment-options")]
    public string? ExperimentOptions { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}