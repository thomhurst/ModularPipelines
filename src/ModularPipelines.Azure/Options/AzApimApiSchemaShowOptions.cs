using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apim", "api", "schema", "show")]
public record AzApimApiSchemaShowOptions(
[property: CliOption("--api-id")] string ApiId,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--schema-id")] string SchemaId,
[property: CliOption("--service-name")] string ServiceName
) : AzOptions;