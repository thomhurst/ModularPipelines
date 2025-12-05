using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datazone", "create-group-profile")]
public record AwsDatazoneCreateGroupProfileOptions(
[property: CliOption("--domain-identifier")] string DomainIdentifier,
[property: CliOption("--group-identifier")] string GroupIdentifier
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}