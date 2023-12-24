using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("schemas", "export-schema")]
public record AwsSchemasExportSchemaOptions(
[property: CommandSwitch("--registry-name")] string RegistryName,
[property: CommandSwitch("--schema-name")] string SchemaName,
[property: CommandSwitch("--type")] string Type
) : AwsOptions
{
    [CommandSwitch("--schema-version")]
    public string? SchemaVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}