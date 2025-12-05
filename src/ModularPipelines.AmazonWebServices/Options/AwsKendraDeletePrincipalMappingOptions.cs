using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kendra", "delete-principal-mapping")]
public record AwsKendraDeletePrincipalMappingOptions(
[property: CliOption("--index-id")] string IndexId,
[property: CliOption("--group-id")] string GroupId
) : AwsOptions
{
    [CliOption("--data-source-id")]
    public string? DataSourceId { get; set; }

    [CliOption("--ordering-id")]
    public long? OrderingId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}