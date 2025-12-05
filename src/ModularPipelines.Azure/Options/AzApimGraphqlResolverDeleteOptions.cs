using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("apim", "graphql", "resolver", "delete")]
public record AzApimGraphqlResolverDeleteOptions(
[property: CliOption("--api-id")] string ApiId,
[property: CliOption("--resolver-id")] string ResolverId,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service-name")] string ServiceName
) : AzOptions
{
    [CliOption("--if-match")]
    public string? IfMatch { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}