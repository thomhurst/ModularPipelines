using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("timestream-write", "write-records")]
public record AwsTimestreamWriteWriteRecordsOptions(
[property: CliOption("--database-name")] string DatabaseName,
[property: CliOption("--table-name")] string TableName,
[property: CliOption("--records")] string[] Records
) : AwsOptions
{
    [CliOption("--common-attributes")]
    public string? CommonAttributes { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}