using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("imagebuilder", "create-image")]
public record AwsImagebuilderCreateImageOptions(
[property: CommandSwitch("--infrastructure-configuration-arn")] string InfrastructureConfigurationArn
) : AwsOptions
{
    [CommandSwitch("--image-recipe-arn")]
    public string? ImageRecipeArn { get; set; }

    [CommandSwitch("--container-recipe-arn")]
    public string? ContainerRecipeArn { get; set; }

    [CommandSwitch("--distribution-configuration-arn")]
    public string? DistributionConfigurationArn { get; set; }

    [CommandSwitch("--image-tests-configuration")]
    public string? ImageTestsConfiguration { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--image-scanning-configuration")]
    public string? ImageScanningConfiguration { get; set; }

    [CommandSwitch("--workflows")]
    public string[]? Workflows { get; set; }

    [CommandSwitch("--execution-role")]
    public string? ExecutionRole { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}