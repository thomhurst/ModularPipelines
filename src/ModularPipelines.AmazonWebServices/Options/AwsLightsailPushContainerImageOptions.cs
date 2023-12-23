using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lightsail", "push-container-image")]
public record AwsLightsailPushContainerImageOptions(
[property: CommandSwitch("--service-name")] string ServiceName,
[property: CommandSwitch("--image")] string Image,
[property: CommandSwitch("--label")] string Label
) : AwsOptions;