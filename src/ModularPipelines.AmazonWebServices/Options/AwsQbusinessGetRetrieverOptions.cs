using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("qbusiness", "get-retriever")]
public record AwsQbusinessGetRetrieverOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--retriever-id")] string RetrieverId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}