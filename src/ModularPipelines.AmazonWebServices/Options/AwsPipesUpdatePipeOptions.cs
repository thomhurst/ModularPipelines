using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pipes", "update-pipe")]
public record AwsPipesUpdatePipeOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--role-arn")] string RoleArn
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

    [CliOption("--target")]
    public string? Target { get; set; }

    [CliOption("--target-parameters")]
    public string? TargetParameters { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}