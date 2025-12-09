using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lakeformation", "grant-permissions")]
public record AwsLakeformationGrantPermissionsOptions(
[property: CliOption("--principal")] string Principal,
[property: CliOption("--resource")] string Resource,
[property: CliOption("--permissions")] string[] Permissions
) : AwsOptions
{
    [CliOption("--catalog-id")]
    public string? CatalogId { get; set; }

    [CliOption("--permissions-with-grant-option")]
    public string[]? PermissionsWithGrantOption { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}