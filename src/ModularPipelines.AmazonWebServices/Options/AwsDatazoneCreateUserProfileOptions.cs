using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datazone", "create-user-profile")]
public record AwsDatazoneCreateUserProfileOptions(
[property: CliOption("--domain-identifier")] string DomainIdentifier,
[property: CliOption("--user-identifier")] string UserIdentifier
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--user-type")]
    public string? UserType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}