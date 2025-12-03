using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apim", "graphql", "resolver", "policy", "list")]
public record AzApimGraphqlResolverPolicyListOptions(
[property: CliOption("--api-id")] string ApiId,
[property: CliOption("--resolver-id")] string ResolverId,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service-name")] string ServiceName
) : AzOptions;