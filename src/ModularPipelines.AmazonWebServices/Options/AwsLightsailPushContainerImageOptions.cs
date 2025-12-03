using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lightsail", "push-container-image")]
public record AwsLightsailPushContainerImageOptions(
[property: CliOption("--service-name")] string ServiceName,
[property: CliOption("--image")] string Image,
[property: CliOption("--label")] string Label
) : AwsOptions;