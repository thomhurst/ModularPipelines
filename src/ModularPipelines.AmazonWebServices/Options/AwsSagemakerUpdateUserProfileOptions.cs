using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "update-user-profile")]
public record AwsSagemakerUpdateUserProfileOptions(
[property: CommandSwitch("--domain-id")] string DomainId,
[property: CommandSwitch("--user-profile-name")] string UserProfileName
) : AwsOptions
{
    [CommandSwitch("--user-settings")]
    public string? UserSettings { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}