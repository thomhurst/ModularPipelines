using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apim", "graphql", "resolver", "create")]
public record AzApimGraphqlResolverCreateOptions(
[property: CommandSwitch("--api-id")] string ApiId,
[property: CommandSwitch("--display-name")] string DisplayName,
[property: CommandSwitch("--path")] string Path,
[property: CommandSwitch("--resolver-id")] string ResolverId,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--service-name")] string ServiceName
) : AzOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }
}