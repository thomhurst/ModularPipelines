using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apim", "api", "operation", "show")]
public record AzApimApiOperationShowOptions(
[property: CommandSwitch("--api-id")] string ApiId,
[property: CommandSwitch("--operation-id")] string OperationId,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--service-name")] string ServiceName
) : AzOptions;