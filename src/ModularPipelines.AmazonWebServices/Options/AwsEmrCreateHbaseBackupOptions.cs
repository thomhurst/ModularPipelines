using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("emr", "create-hbase-backup")]
public record AwsEmrCreateHbaseBackupOptions(
[property: CommandSwitch("--cluster-id")] string ClusterId,
[property: CommandSwitch("--dir")] string Dir
) : AwsOptions
{
    [BooleanCommandSwitch("--consistent")]
    public bool? Consistent { get; set; }
}