using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ram", "get-resource-share-invitations")]
public record AwsRamGetResourceShareInvitationsOptions : AwsOptions
{
    [CliOption("--resource-share-invitation-arns")]
    public string[]? ResourceShareInvitationArns { get; set; }

    [CliOption("--resource-share-arns")]
    public string[]? ResourceShareArns { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}