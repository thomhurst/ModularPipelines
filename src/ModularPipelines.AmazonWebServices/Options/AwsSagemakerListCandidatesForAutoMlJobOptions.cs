using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "list-candidates-for-auto-ml-job")]
public record AwsSagemakerListCandidatesForAutoMlJobOptions(
[property: CommandSwitch("--auto-ml-job-name")] string AutoMlJobName
) : AwsOptions
{
    [CommandSwitch("--status-equals")]
    public string? StatusEquals { get; set; }

    [CommandSwitch("--candidate-name-equals")]
    public string? CandidateNameEquals { get; set; }

    [CommandSwitch("--sort-order")]
    public string? SortOrder { get; set; }

    [CommandSwitch("--sort-by")]
    public string? SortBy { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}