using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup", "update-region-settings")]
public record AwsBackupUpdateRegionSettingsOptions : AwsOptions
{
    [CommandSwitch("--resource-type-opt-in-preference")]
    public IEnumerable<KeyValue>? ResourceTypeOptInPreference { get; set; }

    [CommandSwitch("--resource-type-management-preference")]
    public IEnumerable<KeyValue>? ResourceTypeManagementPreference { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}