using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup", "update-region-settings")]
public record AwsBackupUpdateRegionSettingsOptions : AwsOptions
{
    [CliOption("--resource-type-opt-in-preference")]
    public IEnumerable<KeyValue>? ResourceTypeOptInPreference { get; set; }

    [CliOption("--resource-type-management-preference")]
    public IEnumerable<KeyValue>? ResourceTypeManagementPreference { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}