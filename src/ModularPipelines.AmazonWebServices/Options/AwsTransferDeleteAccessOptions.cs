using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("transfer", "delete-access")]
public record AwsTransferDeleteAccessOptions(
[property: CommandSwitch("--server-id")] string ServerId,
[property: CommandSwitch("--external-id")] string ExternalId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}