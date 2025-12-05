using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("imagebuilder", "create-image-pipeline")]
public record AwsImagebuilderCreateImagePipelineOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--infrastructure-configuration-arn")] string InfrastructureConfigurationArn
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--image-recipe-arn")]
    public string? ImageRecipeArn { get; set; }

    [CliOption("--container-recipe-arn")]
    public string? ContainerRecipeArn { get; set; }

    [CliOption("--distribution-configuration-arn")]
    public string? DistributionConfigurationArn { get; set; }

    [CliOption("--image-tests-configuration")]
    public string? ImageTestsConfiguration { get; set; }

    [CliOption("--schedule")]
    public string? Schedule { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--image-scanning-configuration")]
    public string? ImageScanningConfiguration { get; set; }

    [CliOption("--workflows")]
    public string[]? Workflows { get; set; }

    [CliOption("--execution-role")]
    public string? ExecutionRole { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}