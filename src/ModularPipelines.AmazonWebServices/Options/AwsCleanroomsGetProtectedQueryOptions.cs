using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cleanrooms", "get-protected-query")]
public record AwsCleanroomsGetProtectedQueryOptions(
[property: CliOption("--membership-identifier")] string MembershipIdentifier,
[property: CliOption("--protected-query-identifier")] string ProtectedQueryIdentifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}