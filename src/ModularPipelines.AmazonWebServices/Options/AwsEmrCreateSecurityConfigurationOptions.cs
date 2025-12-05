using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("emr", "create-security-configuration")]
public record AwsEmrCreateSecurityConfigurationOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--security-configuration")] string SecurityConfiguration
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}