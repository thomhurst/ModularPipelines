using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pca-connector-ad", "create-service-principal-name")]
public record AwsPcaConnectorAdCreateServicePrincipalNameOptions(
[property: CliOption("--connector-arn")] string ConnectorArn,
[property: CliOption("--directory-registration-arn")] string DirectoryRegistrationArn
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}