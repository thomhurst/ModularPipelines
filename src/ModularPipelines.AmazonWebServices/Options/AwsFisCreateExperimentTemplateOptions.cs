using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("fis", "create-experiment-template")]
public record AwsFisCreateExperimentTemplateOptions(
[property: CliOption("--description")] string Description,
[property: CliOption("--stop-conditions")] string[] StopConditions,
[property: CliOption("--actions")] IEnumerable<KeyValue> Actions,
[property: CliOption("--role-arn")] string RoleArn
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--targets")]
    public IEnumerable<KeyValue>? Targets { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--log-configuration")]
    public string? LogConfiguration { get; set; }

    [CliOption("--experiment-options")]
    public string? ExperimentOptions { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}