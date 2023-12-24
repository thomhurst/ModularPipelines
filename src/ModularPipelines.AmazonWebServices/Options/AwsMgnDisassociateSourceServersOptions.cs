using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mgn", "disassociate-source-servers")]
public record AwsMgnDisassociateSourceServersOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--source-server-ids")] string[] SourceServerIds
) : AwsOptions
{
    [CommandSwitch("--account-id")]
    public string? AccountId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}