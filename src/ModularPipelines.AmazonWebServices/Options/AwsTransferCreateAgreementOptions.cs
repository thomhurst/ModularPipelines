using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("transfer", "create-agreement")]
public record AwsTransferCreateAgreementOptions(
[property: CommandSwitch("--server-id")] string ServerId,
[property: CommandSwitch("--local-profile-id")] string LocalProfileId,
[property: CommandSwitch("--partner-profile-id")] string PartnerProfileId,
[property: CommandSwitch("--base-directory")] string BaseDirectory,
[property: CommandSwitch("--access-role")] string AccessRole
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--status")]
    public string? Status { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}