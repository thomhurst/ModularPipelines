using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("schemas", "get-code-binding-source")]
public record AwsSchemasGetCodeBindingSourceOptions(
[property: CliOption("--language")] string Language,
[property: CliOption("--registry-name")] string RegistryName,
[property: CliOption("--schema-name")] string SchemaName
) : AwsOptions
{
    [CliOption("--schema-version")]
    public string? SchemaVersion { get; set; }
}