using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "get-package-version")]
public record AwsIotGetPackageVersionOptions(
[property: CommandSwitch("--package-name")] string PackageName,
[property: CommandSwitch("--version-name")] string VersionName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}