using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("qbusiness", "get-group")]
public record AwsQbusinessGetGroupOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--group-name")] string GroupName,
[property: CliOption("--index-id")] string IndexId
) : AwsOptions
{
    [CliOption("--data-source-id")]
    public string? DataSourceId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}