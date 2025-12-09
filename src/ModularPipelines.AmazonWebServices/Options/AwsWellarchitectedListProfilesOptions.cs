using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("wellarchitected", "list-profiles")]
public record AwsWellarchitectedListProfilesOptions : AwsOptions
{
    [CliOption("--profile-name-prefix")]
    public string? ProfileNamePrefix { get; set; }

    [CliOption("--profile-owner-type")]
    public string? ProfileOwnerType { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}