using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cleanrooms", "update-membership")]
public record AwsCleanroomsUpdateMembershipOptions(
[property: CliOption("--membership-identifier")] string MembershipIdentifier
) : AwsOptions
{
    [CliOption("--query-log-status")]
    public string? QueryLogStatus { get; set; }

    [CliOption("--default-result-configuration")]
    public string? DefaultResultConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}