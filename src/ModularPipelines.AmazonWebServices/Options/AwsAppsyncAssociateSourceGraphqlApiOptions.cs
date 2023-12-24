using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appsync", "associate-source-graphql-api")]
public record AwsAppsyncAssociateSourceGraphqlApiOptions(
[property: CommandSwitch("--merged-api-identifier")] string MergedApiIdentifier,
[property: CommandSwitch("--source-api-identifier")] string SourceApiIdentifier
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--source-api-association-config")]
    public string? SourceApiAssociationConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}