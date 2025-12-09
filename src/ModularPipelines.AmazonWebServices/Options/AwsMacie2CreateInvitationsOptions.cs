using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("macie2", "create-invitations")]
public record AwsMacie2CreateInvitationsOptions(
[property: CliOption("--account-ids")] string[] AccountIds
) : AwsOptions
{
    [CliOption("--message")]
    public string? Message { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}