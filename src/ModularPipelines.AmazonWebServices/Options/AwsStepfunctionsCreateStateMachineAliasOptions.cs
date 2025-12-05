using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("stepfunctions", "create-state-machine-alias")]
public record AwsStepfunctionsCreateStateMachineAliasOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--routing-configuration")] string[] RoutingConfiguration
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}