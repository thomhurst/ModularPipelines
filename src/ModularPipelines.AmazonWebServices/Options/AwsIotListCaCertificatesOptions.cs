using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "list-ca-certificates")]
public record AwsIotListCaCertificatesOptions : AwsOptions
{
    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--template-name")]
    public string? TemplateName { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}