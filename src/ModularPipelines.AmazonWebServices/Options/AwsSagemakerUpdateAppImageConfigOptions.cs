using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "update-app-image-config")]
public record AwsSagemakerUpdateAppImageConfigOptions(
[property: CommandSwitch("--app-image-config-name")] string AppImageConfigName
) : AwsOptions
{
    [CommandSwitch("--kernel-gateway-image-config")]
    public string? KernelGatewayImageConfig { get; set; }

    [CommandSwitch("--jupyter-lab-app-image-config")]
    public string? JupyterLabAppImageConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}