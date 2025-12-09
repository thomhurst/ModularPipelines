using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workmail", "register-mail-domain")]
public record AwsWorkmailRegisterMailDomainOptions(
[property: CliOption("--organization-id")] string OrganizationId,
[property: CliOption("--domain-name")] string DomainName
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}