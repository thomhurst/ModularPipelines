using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apim", "product", "api", "check")]
public record AzApimProductApiCheckOptions(
[property: CliOption("--api-id")] string ApiId,
[property: CliOption("--product-id")] string ProductId,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service-name")] string ServiceName
) : AzOptions;