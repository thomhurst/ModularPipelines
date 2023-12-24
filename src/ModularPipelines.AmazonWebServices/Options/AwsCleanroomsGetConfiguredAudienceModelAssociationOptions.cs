using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cleanrooms", "get-configured-audience-model-association")]
public record AwsCleanroomsGetConfiguredAudienceModelAssociationOptions(
[property: CommandSwitch("--configured-audience-model-association-identifier")] string ConfiguredAudienceModelAssociationIdentifier,
[property: CommandSwitch("--membership-identifier")] string MembershipIdentifier
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}