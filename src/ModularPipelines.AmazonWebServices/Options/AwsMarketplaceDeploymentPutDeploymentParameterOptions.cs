using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("marketplace-deployment", "put-deployment-parameter")]
public record AwsMarketplaceDeploymentPutDeploymentParameterOptions(
[property: CommandSwitch("--agreement-id")] string AgreementId,
[property: CommandSwitch("--catalog")] string Catalog,
[property: CommandSwitch("--deployment-parameter")] string DeploymentParameter,
[property: CommandSwitch("--product-id")] string ProductId
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--expiration-date")]
    public long? ExpirationDate { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}