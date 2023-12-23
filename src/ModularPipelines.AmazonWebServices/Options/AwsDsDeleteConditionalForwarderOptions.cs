using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ds", "delete-conditional-forwarder")]
public record AwsDsDeleteConditionalForwarderOptions(
[property: CommandSwitch("--directory-id")] string DirectoryId,
[property: CommandSwitch("--remote-domain-name")] string RemoteDomainName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}