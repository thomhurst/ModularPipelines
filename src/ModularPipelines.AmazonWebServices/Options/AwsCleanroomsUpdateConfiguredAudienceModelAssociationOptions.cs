using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cleanrooms", "update-configured-audience-model-association")]
public record AwsCleanroomsUpdateConfiguredAudienceModelAssociationOptions(
[property: CliOption("--configured-audience-model-association-identifier")] string ConfiguredAudienceModelAssociationIdentifier,
[property: CliOption("--membership-identifier")] string MembershipIdentifier
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}