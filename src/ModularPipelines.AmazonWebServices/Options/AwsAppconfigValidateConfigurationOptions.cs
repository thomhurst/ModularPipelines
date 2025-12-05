using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appconfig", "validate-configuration")]
public record AwsAppconfigValidateConfigurationOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--configuration-profile-id")] string ConfigurationProfileId,
[property: CliOption("--configuration-version")] string ConfigurationVersion
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}