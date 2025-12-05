using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("organizations", "move-account")]
public record AwsOrganizationsMoveAccountOptions(
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--source-parent-id")] string SourceParentId,
[property: CliOption("--destination-parent-id")] string DestinationParentId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}