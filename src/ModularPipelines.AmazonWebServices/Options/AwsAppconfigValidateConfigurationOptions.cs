using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appconfig", "validate-configuration")]
public record AwsAppconfigValidateConfigurationOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--configuration-profile-id")] string ConfigurationProfileId,
[property: CommandSwitch("--configuration-version")] string ConfigurationVersion
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}