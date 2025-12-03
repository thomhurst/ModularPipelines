using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("proton", "delete-repository")]
public record AwsProtonDeleteRepositoryOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--provider")] string Provider
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}