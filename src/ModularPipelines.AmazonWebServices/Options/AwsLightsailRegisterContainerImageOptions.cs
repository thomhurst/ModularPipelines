using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lightsail", "register-container-image")]
public record AwsLightsailRegisterContainerImageOptions(
[property: CommandSwitch("--service-name")] string ServiceName,
[property: CommandSwitch("--label")] string Label,
[property: CommandSwitch("--digest")] string Digest
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}