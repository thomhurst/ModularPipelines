using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pca-connector-ad", "create-service-principal-name")]
public record AwsPcaConnectorAdCreateServicePrincipalNameOptions(
[property: CommandSwitch("--connector-arn")] string ConnectorArn,
[property: CommandSwitch("--directory-registration-arn")] string DirectoryRegistrationArn
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}