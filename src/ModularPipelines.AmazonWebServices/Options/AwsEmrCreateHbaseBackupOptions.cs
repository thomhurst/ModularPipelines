using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("emr", "create-hbase-backup")]
public record AwsEmrCreateHbaseBackupOptions(
[property: CliOption("--cluster-id")] string ClusterId,
[property: CliOption("--dir")] string Dir
) : AwsOptions
{
    [CliFlag("--consistent")]
    public bool? Consistent { get; set; }
}