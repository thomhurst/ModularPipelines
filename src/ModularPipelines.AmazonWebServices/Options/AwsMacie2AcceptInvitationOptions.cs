using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("macie2", "accept-invitation")]
public record AwsMacie2AcceptInvitationOptions(
[property: CliOption("--invitation-id")] string InvitationId
) : AwsOptions
{
    [CliOption("--administrator-account-id")]
    public string? AdministratorAccountId { get; set; }

    [CliOption("--master-account")]
    public string? MasterAccount { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}