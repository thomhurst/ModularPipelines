using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apim", "graphql", "resolver", "create")]
public record AzApimGraphqlResolverCreateOptions(
[property: CliOption("--api-id")] string ApiId,
[property: CliOption("--display-name")] string DisplayName,
[property: CliOption("--path")] string Path,
[property: CliOption("--resolver-id")] string ResolverId,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service-name")] string ServiceName
) : AzOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }
}