using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apim", "graphql", "resolver", "policy", "create")]
public record AzApimGraphqlResolverPolicyCreateOptions(
[property: CliOption("--api-id")] string ApiId,
[property: CliOption("--resolver-id")] string ResolverId,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service-name")] string ServiceName,
[property: CliOption("--value-path")] string ValuePath
) : AzOptions
{
    [CliOption("--policy-format")]
    public string? PolicyFormat { get; set; }
}