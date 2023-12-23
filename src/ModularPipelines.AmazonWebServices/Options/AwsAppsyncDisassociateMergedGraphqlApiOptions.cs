using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appsync", "disassociate-merged-graphql-api")]
public record AwsAppsyncDisassociateMergedGraphqlApiOptions(
[property: CommandSwitch("--source-api-identifier")] string SourceApiIdentifier,
[property: CommandSwitch("--association-id")] string AssociationId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}