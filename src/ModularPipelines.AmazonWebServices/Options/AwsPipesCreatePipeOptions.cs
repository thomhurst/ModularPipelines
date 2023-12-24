using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pipes", "create-pipe")]
public record AwsPipesCreatePipeOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--role-arn")] string RoleArn,
[property: CommandSwitch("--source")] string Source,
[property: CommandSwitch("--target")] string Target
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

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--target-parameters")]
    public string? TargetParameters { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}