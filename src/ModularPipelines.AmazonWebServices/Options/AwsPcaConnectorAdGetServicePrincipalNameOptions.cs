using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pca-connector-ad", "get-service-principal-name")]
public record AwsPcaConnectorAdGetServicePrincipalNameOptions(
[property: CommandSwitch("--connector-arn")] string ConnectorArn,
[property: CommandSwitch("--directory-registration-arn")] string DirectoryRegistrationArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}