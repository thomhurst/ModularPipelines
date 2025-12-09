using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudformation", "deregister-type")]
public record AwsCloudformationDeregisterTypeOptions : AwsOptions
{
    [CliOption("--arn")]
    public string? Arn { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }

    [CliOption("--type-name")]
    public string? TypeName { get; set; }

    [CliOption("--version-id")]
    public string? VersionId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}