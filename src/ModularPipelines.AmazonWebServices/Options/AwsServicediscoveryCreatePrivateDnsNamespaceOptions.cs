using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicediscovery", "create-private-dns-namespace")]
public record AwsServicediscoveryCreatePrivateDnsNamespaceOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--vpc")] string Vpc
) : AwsOptions
{
    [CommandSwitch("--creator-request-id")]
    public string? CreatorRequestId { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--properties")]
    public string? Properties { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}