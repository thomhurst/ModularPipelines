using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("es", "create-package")]
public record AwsEsCreatePackageOptions(
[property: CliOption("--package-name")] string PackageName,
[property: CliOption("--package-type")] string PackageType,
[property: CliOption("--package-source")] string PackageSource
) : AwsOptions
{
    [CliOption("--package-description")]
    public string? PackageDescription { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}