using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "describe-user-profile")]
public record AwsSagemakerDescribeUserProfileOptions(
[property: CommandSwitch("--domain-id")] string DomainId,
[property: CommandSwitch("--user-profile-name")] string UserProfileName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}