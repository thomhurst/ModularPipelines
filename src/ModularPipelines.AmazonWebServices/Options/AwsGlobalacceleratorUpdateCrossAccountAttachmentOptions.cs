using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("globalaccelerator", "update-cross-account-attachment")]
public record AwsGlobalacceleratorUpdateCrossAccountAttachmentOptions(
[property: CommandSwitch("--attachment-arn")] string AttachmentArn
) : AwsOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--add-principals")]
    public string[]? AddPrincipals { get; set; }

    [CommandSwitch("--remove-principals")]
    public string[]? RemovePrincipals { get; set; }

    [CommandSwitch("--add-resources")]
    public string[]? AddResources { get; set; }

    [CommandSwitch("--remove-resources")]
    public string[]? RemoveResources { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}