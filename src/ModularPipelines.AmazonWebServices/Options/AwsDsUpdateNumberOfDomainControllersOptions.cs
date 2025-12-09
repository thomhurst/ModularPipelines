using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ds", "update-number-of-domain-controllers")]
public record AwsDsUpdateNumberOfDomainControllersOptions(
[property: CliOption("--directory-id")] string DirectoryId,
[property: CliOption("--desired-number")] int DesiredNumber
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}