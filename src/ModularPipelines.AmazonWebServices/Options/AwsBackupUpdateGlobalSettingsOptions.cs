using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup", "update-global-settings")]
public record AwsBackupUpdateGlobalSettingsOptions : AwsOptions
{
    [CliOption("--global-settings")]
    public IEnumerable<KeyValue>? GlobalSettings { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}