using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("schemas", "get-code-binding-source")]
public record AwsSchemasGetCodeBindingSourceOptions(
[property: CommandSwitch("--language")] string Language,
[property: CommandSwitch("--registry-name")] string RegistryName,
[property: CommandSwitch("--schema-name")] string SchemaName
) : AwsOptions
{
    [CommandSwitch("--schema-version")]
    public string? SchemaVersion { get; set; }
}