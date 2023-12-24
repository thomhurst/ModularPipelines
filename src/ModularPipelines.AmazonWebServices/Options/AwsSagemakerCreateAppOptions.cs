using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "create-app")]
public record AwsSagemakerCreateAppOptions(
[property: CommandSwitch("--domain-id")] string DomainId,
[property: CommandSwitch("--app-type")] string AppType,
[property: CommandSwitch("--app-name")] string AppName
) : AwsOptions
{
    [CommandSwitch("--user-profile-name")]
    public string? UserProfileName { get; set; }

    [CommandSwitch("--space-name")]
    public string? SpaceName { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--resource-spec")]
    public string? ResourceSpec { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}