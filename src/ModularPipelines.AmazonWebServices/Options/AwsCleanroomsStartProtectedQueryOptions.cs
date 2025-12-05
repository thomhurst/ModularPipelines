using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cleanrooms", "start-protected-query")]
public record AwsCleanroomsStartProtectedQueryOptions(
[property: CliOption("--type")] string Type,
[property: CliOption("--membership-identifier")] string MembershipIdentifier,
[property: CliOption("--sql-parameters")] string SqlParameters
) : AwsOptions
{
    [CliOption("--result-configuration")]
    public string? ResultConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}