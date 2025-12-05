using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("machinelearning", "wait", "ml-model-available")]
public record AwsMachinelearningWaitMlModelAvailableOptions : AwsOptions
{
    [CliOption("--filter-variable")]
    public string? FilterVariable { get; set; }

    [CliOption("--eq")]
    public string? Eq { get; set; }

    [CliOption("--gt")]
    public string? Gt { get; set; }

    [CliOption("--lt")]
    public string? Lt { get; set; }

    [CliOption("--ge")]
    public string? Ge { get; set; }

    [CliOption("--le")]
    public string? Le { get; set; }

    [CliOption("--ne")]
    public string? Ne { get; set; }

    [CliOption("--prefix")]
    public string? Prefix { get; set; }

    [CliOption("--sort-order")]
    public string? SortOrder { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}