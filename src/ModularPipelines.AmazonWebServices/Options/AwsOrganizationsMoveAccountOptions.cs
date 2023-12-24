using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("organizations", "move-account")]
public record AwsOrganizationsMoveAccountOptions(
[property: CommandSwitch("--account-id")] string AccountId,
[property: CommandSwitch("--source-parent-id")] string SourceParentId,
[property: CommandSwitch("--destination-parent-id")] string DestinationParentId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}