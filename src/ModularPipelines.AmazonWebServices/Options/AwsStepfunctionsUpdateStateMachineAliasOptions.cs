using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("stepfunctions", "update-state-machine-alias")]
public record AwsStepfunctionsUpdateStateMachineAliasOptions(
[property: CliOption("--state-machine-alias-arn")] string StateMachineAliasArn
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--routing-configuration")]
    public string[]? RoutingConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}