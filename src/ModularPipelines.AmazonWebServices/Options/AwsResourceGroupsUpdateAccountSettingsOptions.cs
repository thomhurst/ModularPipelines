using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resource-groups", "update-account-settings")]
public record AwsResourceGroupsUpdateAccountSettingsOptions : AwsOptions
{
    [CommandSwitch("--group-lifecycle-events-desired-status")]
    public string? GroupLifecycleEventsDesiredStatus { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}