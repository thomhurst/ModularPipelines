using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workmail", "describe-entity")]
public record AwsWorkmailDescribeEntityOptions(
[property: CliOption("--organization-id")] string OrganizationId,
[property: CliOption("--email")] string Email
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}