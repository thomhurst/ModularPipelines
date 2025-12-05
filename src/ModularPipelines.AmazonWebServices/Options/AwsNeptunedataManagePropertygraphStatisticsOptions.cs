using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("neptunedata", "manage-propertygraph-statistics")]
public record AwsNeptunedataManagePropertygraphStatisticsOptions : AwsOptions
{
    [CliOption("--mode")]
    public string? Mode { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}