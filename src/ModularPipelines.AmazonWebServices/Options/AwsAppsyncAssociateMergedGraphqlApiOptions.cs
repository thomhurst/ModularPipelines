using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appsync", "associate-merged-graphql-api")]
public record AwsAppsyncAssociateMergedGraphqlApiOptions(
[property: CommandSwitch("--source-api-identifier")] string SourceApiIdentifier,
[property: CommandSwitch("--merged-api-identifier")] string MergedApiIdentifier
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--source-api-association-config")]
    public string? SourceApiAssociationConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}