using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appconfig", "get-configuration-profile")]
public record AwsAppconfigGetConfigurationProfileOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--configuration-profile-id")] string ConfigurationProfileId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}