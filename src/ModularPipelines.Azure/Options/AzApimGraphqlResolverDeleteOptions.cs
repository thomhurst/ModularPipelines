using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apim", "graphql", "resolver", "delete")]
public record AzApimGraphqlResolverDeleteOptions(
[property: CommandSwitch("--api-id")] string ApiId,
[property: CommandSwitch("--resolver-id")] string ResolverId,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--service-name")] string ServiceName
) : AzOptions
{
    [CommandSwitch("--if-match")]
    public string? IfMatch { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}