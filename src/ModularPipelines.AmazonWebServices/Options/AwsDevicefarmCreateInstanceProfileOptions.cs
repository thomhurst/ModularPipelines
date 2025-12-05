using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("devicefarm", "create-instance-profile")]
public record AwsDevicefarmCreateInstanceProfileOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--exclude-app-packages-from-cleanup")]
    public string[]? ExcludeAppPackagesFromCleanup { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}