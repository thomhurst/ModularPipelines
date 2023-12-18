using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apim", "api", "schema", "show")]
public record AzApimApiSchemaShowOptions(
[property: CommandSwitch("--api-id")] string ApiId,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--schema-id")] string SchemaId,
[property: CommandSwitch("--service-name")] string ServiceName
) : AzOptions;