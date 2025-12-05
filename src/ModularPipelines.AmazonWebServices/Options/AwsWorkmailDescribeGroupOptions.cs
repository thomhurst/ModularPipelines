using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workmail", "describe-group")]
public record AwsWorkmailDescribeGroupOptions(
[property: CliOption("--organization-id")] string OrganizationId,
[property: CliOption("--group-id")] string GroupId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}