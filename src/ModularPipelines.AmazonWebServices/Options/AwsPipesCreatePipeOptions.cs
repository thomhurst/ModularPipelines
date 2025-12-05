using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pipes", "create-pipe")]
public record AwsPipesCreatePipeOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--role-arn")] string RoleArn,
[property: CliOption("--source")] string Source,
[property: CliOption("--target")] string Target
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--desired-state")]
    public string? DesiredState { get; set; }

    [CliOption("--enrichment")]
    public string? Enrichment { get; set; }

    [CliOption("--enrichment-parameters")]
    public string? EnrichmentParameters { get; set; }

    [CliOption("--log-configuration")]
    public string? LogConfiguration { get; set; }

    [CliOption("--source-parameters")]
    public string? SourceParameters { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--target-parameters")]
    public string? TargetParameters { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}