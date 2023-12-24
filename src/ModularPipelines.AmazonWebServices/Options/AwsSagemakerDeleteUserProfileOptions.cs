using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "delete-user-profile")]
public record AwsSagemakerDeleteUserProfileOptions(
[property: CommandSwitch("--domain-id")] string DomainId,
[property: CommandSwitch("--user-profile-name")] string UserProfileName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}