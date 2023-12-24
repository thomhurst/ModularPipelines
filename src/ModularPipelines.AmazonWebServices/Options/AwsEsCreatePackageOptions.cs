using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("es", "create-package")]
public record AwsEsCreatePackageOptions(
[property: CommandSwitch("--package-name")] string PackageName,
[property: CommandSwitch("--package-type")] string PackageType,
[property: CommandSwitch("--package-source")] string PackageSource
) : AwsOptions
{
    [CommandSwitch("--package-description")]
    public string? PackageDescription { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}