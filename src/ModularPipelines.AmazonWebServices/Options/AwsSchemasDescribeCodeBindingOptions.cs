using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("schemas", "describe-code-binding")]
public record AwsSchemasDescribeCodeBindingOptions(
[property: CliOption("--language")] string Language,
[property: CliOption("--registry-name")] string RegistryName,
[property: CliOption("--schema-name")] string SchemaName
) : AwsOptions
{
    [CliOption("--schema-version")]
    public string? SchemaVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}