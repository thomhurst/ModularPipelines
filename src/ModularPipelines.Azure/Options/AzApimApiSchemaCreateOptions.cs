using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("apim", "api", "schema", "create")]
public record AzApimApiSchemaCreateOptions(
[property: CliOption("--api-id")] string ApiId,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--schema-id")] string SchemaId,
[property: CliOption("--schema-type")] string SchemaType,
[property: CliOption("--service-name")] string ServiceName
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-type")]
    public string? ResourceType { get; set; }

    [CliOption("--schema-content")]
    public string? SchemaContent { get; set; }

    [CliOption("--schema-name")]
    public string? SchemaName { get; set; }

    [CliOption("--schema-path")]
    public string? SchemaPath { get; set; }
}