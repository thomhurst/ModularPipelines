using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("macie2", "create-invitations")]
public record AwsMacie2CreateInvitationsOptions(
[property: CommandSwitch("--account-ids")] string[] AccountIds
) : AwsOptions
{
    [CommandSwitch("--message")]
    public string? Message { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}