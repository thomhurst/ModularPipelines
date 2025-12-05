using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "create-app-image-config")]
public record AwsSagemakerCreateAppImageConfigOptions(
[property: CliOption("--app-image-config-name")] string AppImageConfigName
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--kernel-gateway-image-config")]
    public string? KernelGatewayImageConfig { get; set; }

    [CliOption("--jupyter-lab-app-image-config")]
    public string? JupyterLabAppImageConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}