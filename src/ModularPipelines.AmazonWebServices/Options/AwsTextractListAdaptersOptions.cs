using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("textract", "list-adapters")]
public record AwsTextractListAdaptersOptions : AwsOptions
{
    [CliOption("--after-creation-time")]
    public long? AfterCreationTime { get; set; }

    [CliOption("--before-creation-time")]
    public long? BeforeCreationTime { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}