using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("es", "update-package")]
public record AwsEsUpdatePackageOptions(
[property: CliOption("--package-id")] string PackageId,
[property: CliOption("--package-source")] string PackageSource
) : AwsOptions
{
    [CliOption("--package-description")]
    public string? PackageDescription { get; set; }

    [CliOption("--commit-message")]
    public string? CommitMessage { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}