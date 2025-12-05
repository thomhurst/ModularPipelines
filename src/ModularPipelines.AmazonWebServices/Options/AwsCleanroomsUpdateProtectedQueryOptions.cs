using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cleanrooms", "update-protected-query")]
public record AwsCleanroomsUpdateProtectedQueryOptions(
[property: CliOption("--membership-identifier")] string MembershipIdentifier,
[property: CliOption("--protected-query-identifier")] string ProtectedQueryIdentifier,
[property: CliOption("--target-status")] string TargetStatus
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}