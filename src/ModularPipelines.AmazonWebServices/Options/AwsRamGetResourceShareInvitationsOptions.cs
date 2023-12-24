using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ram", "get-resource-share-invitations")]
public record AwsRamGetResourceShareInvitationsOptions : AwsOptions
{
    [CommandSwitch("--resource-share-invitation-arns")]
    public string[]? ResourceShareInvitationArns { get; set; }

    [CommandSwitch("--resource-share-arns")]
    public string[]? ResourceShareArns { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}