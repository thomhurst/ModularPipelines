using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("emr", "disable-hbase-backups")]
public record AwsEmrDisableHbaseBackupsOptions(
[property: CliOption("--cluster-id")] string ClusterId
) : AwsOptions
{
    [CliFlag("--full")]
    public bool? Full { get; set; }

    [CliFlag("--incremental")]
    public bool? Incremental { get; set; }
}