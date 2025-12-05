using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "create-package-version")]
public record AwsIotCreatePackageVersionOptions(
[property: CliOption("--package-name")] string PackageName,
[property: CliOption("--version-name")] string VersionName
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--attributes")]
    public IEnumerable<KeyValue>? Attributes { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}