using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apim", "product", "api", "check")]
public record AzApimProductApiCheckOptions(
[property: CommandSwitch("--api-id")] string ApiId,
[property: CommandSwitch("--product-id")] string ProductId,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--service-name")] string ServiceName
) : AzOptions;