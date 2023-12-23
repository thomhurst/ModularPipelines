using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lakeformation", "grant-permissions")]
public record AwsLakeformationGrantPermissionsOptions(
[property: CommandSwitch("--principal")] string Principal,
[property: CommandSwitch("--resource")] string Resource,
[property: CommandSwitch("--permissions")] string[] Permissions
) : AwsOptions
{
    [CommandSwitch("--catalog-id")]
    public string? CatalogId { get; set; }

    [CommandSwitch("--permissions-with-grant-option")]
    public string[]? PermissionsWithGrantOption { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}