using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("timestream-write", "delete-database")]
public record AwsTimestreamWriteDeleteDatabaseOptions(
[property: CliOption("--database-name")] string DatabaseName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}