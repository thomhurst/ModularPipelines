using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("marketplace-catalog", "start-change-set")]
public record AwsMarketplaceCatalogStartChangeSetOptions(
[property: CommandSwitch("--catalog")] string Catalog,
[property: CommandSwitch("--change-set")] string[] ChangeSet
) : AwsOptions
{
    [CommandSwitch("--change-set-name")]
    public string? ChangeSetName { get; set; }

    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--change-set-tags")]
    public string[]? ChangeSetTags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}