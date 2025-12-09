using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deploy", "list-on-premises-instances")]
public record AwsDeployListOnPremisesInstancesOptions : AwsOptions
{
    [CliOption("--registration-status")]
    public string? RegistrationStatus { get; set; }

    [CliOption("--tag-filters")]
    public string[]? TagFilters { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}