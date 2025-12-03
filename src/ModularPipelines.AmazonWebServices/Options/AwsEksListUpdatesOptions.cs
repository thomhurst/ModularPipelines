using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eks", "list-updates")]
public record AwsEksListUpdatesOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--nodegroup-name")]
    public string? NodegroupName { get; set; }

    [CliOption("--addon-name")]
    public string? AddonName { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}