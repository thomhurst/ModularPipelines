using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lakeformation", "get-effective-permissions-for-path")]
public record AwsLakeformationGetEffectivePermissionsForPathOptions(
[property: CommandSwitch("--resource-arn")] string ResourceArn
) : AwsOptions
{
    [CommandSwitch("--catalog-id")]
    public string? CatalogId { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}