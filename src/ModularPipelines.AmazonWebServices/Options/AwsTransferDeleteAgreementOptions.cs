using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("transfer", "delete-agreement")]
public record AwsTransferDeleteAgreementOptions(
[property: CommandSwitch("--agreement-id")] string AgreementId,
[property: CommandSwitch("--server-id")] string ServerId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}