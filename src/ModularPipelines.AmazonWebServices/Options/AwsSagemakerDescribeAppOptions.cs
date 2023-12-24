using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "describe-app")]
public record AwsSagemakerDescribeAppOptions(
[property: CommandSwitch("--domain-id")] string DomainId,
[property: CommandSwitch("--app-type")] string AppType,
[property: CommandSwitch("--app-name")] string AppName
) : AwsOptions
{
    [CommandSwitch("--user-profile-name")]
    public string? UserProfileName { get; set; }

    [CommandSwitch("--space-name")]
    public string? SpaceName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}