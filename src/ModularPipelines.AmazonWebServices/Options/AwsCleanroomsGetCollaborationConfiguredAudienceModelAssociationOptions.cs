using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cleanrooms", "get-collaboration-configured-audience-model-association")]
public record AwsCleanroomsGetCollaborationConfiguredAudienceModelAssociationOptions(
[property: CliOption("--collaboration-identifier")] string CollaborationIdentifier,
[property: CliOption("--configured-audience-model-association-identifier")] string ConfiguredAudienceModelAssociationIdentifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}