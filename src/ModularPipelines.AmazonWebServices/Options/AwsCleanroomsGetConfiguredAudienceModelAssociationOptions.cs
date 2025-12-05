using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cleanrooms", "get-configured-audience-model-association")]
public record AwsCleanroomsGetConfiguredAudienceModelAssociationOptions(
[property: CliOption("--configured-audience-model-association-identifier")] string ConfiguredAudienceModelAssociationIdentifier,
[property: CliOption("--membership-identifier")] string MembershipIdentifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}