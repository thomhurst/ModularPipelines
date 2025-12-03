using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lightsail", "register-container-image")]
public record AwsLightsailRegisterContainerImageOptions(
[property: CliOption("--service-name")] string ServiceName,
[property: CliOption("--label")] string Label,
[property: CliOption("--digest")] string Digest
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}