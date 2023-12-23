using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("transfer", "update-agreement")]
public record AwsTransferUpdateAgreementOptions(
[property: CommandSwitch("--agreement-id")] string AgreementId,
[property: CommandSwitch("--server-id")] string ServerId
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--status")]
    public string? Status { get; set; }

    [CommandSwitch("--local-profile-id")]
    public string? LocalProfileId { get; set; }

    [CommandSwitch("--partner-profile-id")]
    public string? PartnerProfileId { get; set; }

    [CommandSwitch("--base-directory")]
    public string? BaseDirectory { get; set; }

    [CommandSwitch("--access-role")]
    public string? AccessRole { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}