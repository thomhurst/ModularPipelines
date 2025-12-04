using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("apim", "api", "schema", "get-etag")]
public record AzApimApiSchemaGetEtagOptions(
[property: CliOption("--api-id")] string ApiId,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--schema-id")] string SchemaId,
[property: CliOption("--service-name")] string ServiceName
) : AzOptions;