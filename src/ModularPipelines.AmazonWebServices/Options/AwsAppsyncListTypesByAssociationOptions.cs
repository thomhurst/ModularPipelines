using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appsync", "list-types-by-association")]
public record AwsAppsyncListTypesByAssociationOptions(
[property: CommandSwitch("--merged-api-identifier")] string MergedApiIdentifier,
[property: CommandSwitch("--association-id")] string AssociationId,
[property: CommandSwitch("--format")] string Format
) : AwsOptions
{
    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}