using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("marketplace-catalog", "cancel-change-set")]
public record AwsMarketplaceCatalogCancelChangeSetOptions(
[property: CommandSwitch("--catalog")] string Catalog,
[property: CommandSwitch("--change-set-id")] string ChangeSetId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}