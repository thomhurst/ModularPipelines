using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resiliencehub", "import-resources-to-draft-app-version")]
public record AwsResiliencehubImportResourcesToDraftAppVersionOptions(
[property: CliOption("--app-arn")] string AppArn
) : AwsOptions
{
    [CliOption("--eks-sources")]
    public string[]? EksSources { get; set; }

    [CliOption("--import-strategy")]
    public string? ImportStrategy { get; set; }

    [CliOption("--source-arns")]
    public string[]? SourceArns { get; set; }

    [CliOption("--terraform-sources")]
    public string[]? TerraformSources { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}