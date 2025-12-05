using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codeguru-security", "create-upload-url")]
public record AwsCodeguruSecurityCreateUploadUrlOptions(
[property: CliOption("--scan-name")] string ScanName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}