using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pca-connector-ad", "delete-service-principal-name")]
public record AwsPcaConnectorAdDeleteServicePrincipalNameOptions(
[property: CommandSwitch("--connector-arn")] string ConnectorArn,
[property: CommandSwitch("--directory-registration-arn")] string DirectoryRegistrationArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}