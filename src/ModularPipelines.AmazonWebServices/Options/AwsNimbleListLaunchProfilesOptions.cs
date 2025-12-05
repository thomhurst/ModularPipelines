using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("nimble", "list-launch-profiles")]
public record AwsNimbleListLaunchProfilesOptions(
[property: CliOption("--studio-id")] string StudioId
) : AwsOptions
{
    [CliOption("--principal-id")]
    public string? PrincipalId { get; set; }

    [CliOption("--states")]
    public string[]? States { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}