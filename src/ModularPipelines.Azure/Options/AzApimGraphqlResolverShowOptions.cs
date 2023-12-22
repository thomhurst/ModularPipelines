using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apim", "graphql", "resolver", "show")]
public record AzApimGraphqlResolverShowOptions(
[property: CommandSwitch("--api-id")] string ApiId,
[property: CommandSwitch("--resolver-id")] string ResolverId,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--service-name")] string ServiceName
) : AzOptions;