using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ds", "create-alias")]
public record AwsDsCreateAliasOptions(
[property: CliOption("--directory-id")] string DirectoryId,
[property: CliOption("--alias")] string Alias
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}