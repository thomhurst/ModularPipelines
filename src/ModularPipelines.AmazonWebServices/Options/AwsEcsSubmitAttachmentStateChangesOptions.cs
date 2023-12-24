using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ecs", "submit-attachment-state-changes")]
public record AwsEcsSubmitAttachmentStateChangesOptions(
[property: CommandSwitch("--attachments")] string[] Attachments
) : AwsOptions
{
    [CommandSwitch("--cluster")]
    public string? Cluster { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}