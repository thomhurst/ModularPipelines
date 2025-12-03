using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm", "delete-inventory")]
public record AwsSsmDeleteInventoryOptions(
[property: CliOption("--type-name")] string TypeName
) : AwsOptions
{
    [CliOption("--schema-delete-option")]
    public string? SchemaDeleteOption { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}