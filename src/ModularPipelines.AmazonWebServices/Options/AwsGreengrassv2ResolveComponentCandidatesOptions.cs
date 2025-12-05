using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("greengrassv2", "resolve-component-candidates")]
public record AwsGreengrassv2ResolveComponentCandidatesOptions : AwsOptions
{
    [CliOption("--platform")]
    public string? Platform { get; set; }

    [CliOption("--component-candidates")]
    public string[]? ComponentCandidates { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}