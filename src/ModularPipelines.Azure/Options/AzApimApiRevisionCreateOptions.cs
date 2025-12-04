using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("apim", "api", "revision", "create")]
public record AzApimApiRevisionCreateOptions(
[property: CliOption("--api-id")] string ApiId,
[property: CliOption("--api-revision")] string ApiRevision,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service-name")] string ServiceName
) : AzOptions
{
    [CliOption("--api-revision-description")]
    public string? ApiRevisionDescription { get; set; }
}