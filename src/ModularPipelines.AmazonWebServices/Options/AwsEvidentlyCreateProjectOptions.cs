using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("evidently", "create-project")]
public record AwsEvidentlyCreateProjectOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--app-config-resource")]
    public string? AppConfigResource { get; set; }

    [CliOption("--data-delivery")]
    public string? DataDelivery { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}