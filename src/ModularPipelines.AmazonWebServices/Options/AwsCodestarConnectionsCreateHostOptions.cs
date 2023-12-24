using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codestar-connections", "create-host")]
public record AwsCodestarConnectionsCreateHostOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--provider-type")] string ProviderType,
[property: CommandSwitch("--provider-endpoint")] string ProviderEndpoint
) : AwsOptions
{
    [CommandSwitch("--vpc-configuration")]
    public string? VpcConfiguration { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}