using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cleanrooms", "get-configured-table-association")]
public record AwsCleanroomsGetConfiguredTableAssociationOptions(
[property: CliOption("--configured-table-association-identifier")] string ConfiguredTableAssociationIdentifier,
[property: CliOption("--membership-identifier")] string MembershipIdentifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}