using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("securitylake", "delete-custom-log-source")]
public record AwsSecuritylakeDeleteCustomLogSourceOptions(
[property: CliOption("--source-name")] string SourceName
) : AwsOptions
{
    [CliOption("--source-version")]
    public string? SourceVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}