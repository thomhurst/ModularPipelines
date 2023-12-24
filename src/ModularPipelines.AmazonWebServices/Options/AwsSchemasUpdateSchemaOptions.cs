using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("schemas", "update-schema")]
public record AwsSchemasUpdateSchemaOptions(
[property: CommandSwitch("--registry-name")] string RegistryName,
[property: CommandSwitch("--schema-name")] string SchemaName
) : AwsOptions
{
    [CommandSwitch("--client-token-id")]
    public string? ClientTokenId { get; set; }

    [CommandSwitch("--content")]
    public string? Content { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--type")]
    public string? Type { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}