using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appsync", "disassociate-source-graphql-api")]
public record AwsAppsyncDisassociateSourceGraphqlApiOptions(
[property: CommandSwitch("--merged-api-identifier")] string MergedApiIdentifier,
[property: CommandSwitch("--association-id")] string AssociationId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}