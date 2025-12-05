using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pca-connector-ad", "delete-service-principal-name")]
public record AwsPcaConnectorAdDeleteServicePrincipalNameOptions(
[property: CliOption("--connector-arn")] string ConnectorArn,
[property: CliOption("--directory-registration-arn")] string DirectoryRegistrationArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}