using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ds", "disable-client-authentication")]
public record AwsDsDisableClientAuthenticationOptions(
[property: CliOption("--directory-id")] string DirectoryId,
[property: CliOption("--type")] string Type
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}