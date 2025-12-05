using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lightsail", "get-contact-methods")]
public record AwsLightsailGetContactMethodsOptions : AwsOptions
{
    [CliOption("--protocols")]
    public string[]? Protocols { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}