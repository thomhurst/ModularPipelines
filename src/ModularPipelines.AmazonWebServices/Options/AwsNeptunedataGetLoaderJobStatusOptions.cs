using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("neptunedata", "get-loader-job-status")]
public record AwsNeptunedataGetLoaderJobStatusOptions(
[property: CommandSwitch("--load-id")] string LoadId
) : AwsOptions
{
    [CommandSwitch("--page")]
    public int? Page { get; set; }

    [CommandSwitch("--errors-per-page")]
    public int? ErrorsPerPage { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}