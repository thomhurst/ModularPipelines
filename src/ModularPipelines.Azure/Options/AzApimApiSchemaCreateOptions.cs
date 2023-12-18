using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apim", "api", "schema", "create")]
public record AzApimApiSchemaCreateOptions(
[property: CommandSwitch("--api-id")] string ApiId,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--schema-id")] string SchemaId,
[property: CommandSwitch("--schema-type")] string SchemaType,
[property: CommandSwitch("--service-name")] string ServiceName
) : AzOptions
{
    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--resource-type")]
    public string? ResourceType { get; set; }

    [CommandSwitch("--schema-content")]
    public string? SchemaContent { get; set; }

    [CommandSwitch("--schema-name")]
    public string? SchemaName { get; set; }

    [CommandSwitch("--schema-path")]
    public string? SchemaPath { get; set; }
}