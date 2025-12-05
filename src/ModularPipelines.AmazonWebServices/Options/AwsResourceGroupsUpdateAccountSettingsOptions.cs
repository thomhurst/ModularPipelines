using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resource-groups", "update-account-settings")]
public record AwsResourceGroupsUpdateAccountSettingsOptions : AwsOptions
{
    [CliOption("--group-lifecycle-events-desired-status")]
    public string? GroupLifecycleEventsDesiredStatus { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}