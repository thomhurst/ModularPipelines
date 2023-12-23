using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup", "update-global-settings")]
public record AwsBackupUpdateGlobalSettingsOptions : AwsOptions
{
    [CommandSwitch("--global-settings")]
    public IEnumerable<KeyValue>? GlobalSettings { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}