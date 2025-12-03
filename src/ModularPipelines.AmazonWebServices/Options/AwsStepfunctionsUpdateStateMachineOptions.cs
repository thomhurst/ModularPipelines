using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("stepfunctions", "update-state-machine")]
public record AwsStepfunctionsUpdateStateMachineOptions(
[property: CliOption("--state-machine-arn")] string StateMachineArn
) : AwsOptions
{
    [CliOption("--definition")]
    public string? Definition { get; set; }

    [CliOption("--role-arn")]
    public string? RoleArn { get; set; }

    [CliOption("--logging-configuration")]
    public string? LoggingConfiguration { get; set; }

    [CliOption("--tracing-configuration")]
    public string? TracingConfiguration { get; set; }

    [CliOption("--version-description")]
    public string? VersionDescription { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}