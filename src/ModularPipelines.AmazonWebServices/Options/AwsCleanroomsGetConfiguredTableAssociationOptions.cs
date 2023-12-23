using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cleanrooms", "get-configured-table-association")]
public record AwsCleanroomsGetConfiguredTableAssociationOptions(
[property: CommandSwitch("--configured-table-association-identifier")] string ConfiguredTableAssociationIdentifier,
[property: CommandSwitch("--membership-identifier")] string MembershipIdentifier
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}