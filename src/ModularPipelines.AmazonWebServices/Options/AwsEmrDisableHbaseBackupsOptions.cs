using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("emr", "disable-hbase-backups")]
public record AwsEmrDisableHbaseBackupsOptions(
[property: CommandSwitch("--cluster-id")] string ClusterId
) : AwsOptions
{
    [BooleanCommandSwitch("--full")]
    public bool? Full { get; set; }

    [BooleanCommandSwitch("--incremental")]
    public bool? Incremental { get; set; }
}