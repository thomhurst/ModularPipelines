using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apim", "api", "revision", "create")]
public record AzApimApiRevisionCreateOptions(
[property: CommandSwitch("--api-id")] string ApiId,
[property: CommandSwitch("--api-revision")] string ApiRevision,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--service-name")] string ServiceName
) : AzOptions
{
    [CommandSwitch("--api-revision-description")]
    public string? ApiRevisionDescription { get; set; }
}