using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "update-app-image-config")]
public record AwsSagemakerUpdateAppImageConfigOptions(
[property: CliOption("--app-image-config-name")] string AppImageConfigName
) : AwsOptions
{
    [CliOption("--kernel-gateway-image-config")]
    public string? KernelGatewayImageConfig { get; set; }

    [CliOption("--jupyter-lab-app-image-config")]
    public string? JupyterLabAppImageConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}