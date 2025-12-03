using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("stepfunctions", "create-state-machine")]
public record AwsStepfunctionsCreateStateMachineOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--definition")] string Definition,
[property: CliOption("--role-arn")] string RoleArn
) : AwsOptions
{
    [CliOption("--type")]
    public string? Type { get; set; }

    [CliOption("--logging-configuration")]
    public string? LoggingConfiguration { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--tracing-configuration")]
    public string? TracingConfiguration { get; set; }

    [CliOption("--version-description")]
    public string? VersionDescription { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}