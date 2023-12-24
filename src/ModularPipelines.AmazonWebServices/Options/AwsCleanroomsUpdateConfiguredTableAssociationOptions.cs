using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cleanrooms", "update-configured-table-association")]
public record AwsCleanroomsUpdateConfiguredTableAssociationOptions(
[property: CommandSwitch("--configured-table-association-identifier")] string ConfiguredTableAssociationIdentifier,
[property: CommandSwitch("--membership-identifier")] string MembershipIdentifier
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--role-arn")]
    public string? RoleArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}