using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lightsail", "create-contact-method")]
public record AwsLightsailCreateContactMethodOptions(
[property: CliOption("--protocol")] string Protocol,
[property: CliOption("--contact-endpoint")] string ContactEndpoint
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}