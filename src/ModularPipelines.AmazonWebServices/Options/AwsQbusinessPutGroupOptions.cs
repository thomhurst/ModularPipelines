using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("qbusiness", "put-group")]
public record AwsQbusinessPutGroupOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--group-members")] string GroupMembers,
[property: CliOption("--group-name")] string GroupName,
[property: CliOption("--index-id")] string IndexId,
[property: CliOption("--type")] string Type
) : AwsOptions
{
    [CliOption("--data-source-id")]
    public string? DataSourceId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}