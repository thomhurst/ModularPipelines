using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apim", "graphql", "resolver", "policy", "create")]
public record AzApimGraphqlResolverPolicyCreateOptions(
[property: CommandSwitch("--api-id")] string ApiId,
[property: CommandSwitch("--resolver-id")] string ResolverId,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--service-name")] string ServiceName,
[property: CommandSwitch("--value-path")] string ValuePath
) : AzOptions
{
    [CommandSwitch("--policy-format")]
    public string? PolicyFormat { get; set; }
}