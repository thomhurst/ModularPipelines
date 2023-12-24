using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("opensearch", "create-vpc-endpoint")]
public record AwsOpensearchCreateVpcEndpointOptions(
[property: CommandSwitch("--domain-arn")] string DomainArn,
[property: CommandSwitch("--vpc-options")] string VpcOptions
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}