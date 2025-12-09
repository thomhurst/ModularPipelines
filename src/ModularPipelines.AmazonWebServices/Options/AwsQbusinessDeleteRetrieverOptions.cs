using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("qbusiness", "delete-retriever")]
public record AwsQbusinessDeleteRetrieverOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--retriever-id")] string RetrieverId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}