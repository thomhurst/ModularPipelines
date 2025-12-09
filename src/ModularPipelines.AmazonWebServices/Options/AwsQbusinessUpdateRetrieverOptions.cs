using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("qbusiness", "update-retriever")]
public record AwsQbusinessUpdateRetrieverOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--retriever-id")] string RetrieverId
) : AwsOptions
{
    [CliOption("--configuration")]
    public string? Configuration { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--role-arn")]
    public string? RoleArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}