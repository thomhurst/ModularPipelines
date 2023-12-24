using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pipes", "update-pipe")]
public record AwsPipesUpdatePipeOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--role-arn")] string RoleArn
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--desired-state")]
    public string? DesiredState { get; set; }

    [CommandSwitch("--enrichment")]
    public string? Enrichment { get; set; }

    [CommandSwitch("--enrichment-parameters")]
    public string? EnrichmentParameters { get; set; }

    [CommandSwitch("--log-configuration")]
    public string? LogConfiguration { get; set; }

    [CommandSwitch("--source-parameters")]
    public string? SourceParameters { get; set; }

    [CommandSwitch("--target")]
    public string? Target { get; set; }

    [CommandSwitch("--target-parameters")]
    public string? TargetParameters { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}