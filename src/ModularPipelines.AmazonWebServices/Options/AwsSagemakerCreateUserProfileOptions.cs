using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "create-user-profile")]
public record AwsSagemakerCreateUserProfileOptions(
[property: CommandSwitch("--domain-id")] string DomainId,
[property: CommandSwitch("--user-profile-name")] string UserProfileName
) : AwsOptions
{
    [CommandSwitch("--single-sign-on-user-identifier")]
    public string? SingleSignOnUserIdentifier { get; set; }

    [CommandSwitch("--single-sign-on-user-value")]
    public string? SingleSignOnUserValue { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--user-settings")]
    public string? UserSettings { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}