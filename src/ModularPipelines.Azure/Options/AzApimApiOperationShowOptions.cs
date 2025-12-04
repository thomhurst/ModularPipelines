using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("apim", "api", "operation", "show")]
public record AzApimApiOperationShowOptions(
[property: CliOption("--api-id")] string ApiId,
[property: CliOption("--operation-id")] string OperationId,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service-name")] string ServiceName
) : AzOptions;