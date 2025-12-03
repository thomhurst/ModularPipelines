using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("globalaccelerator", "update-cross-account-attachment")]
public record AwsGlobalacceleratorUpdateCrossAccountAttachmentOptions(
[property: CliOption("--attachment-arn")] string AttachmentArn
) : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--add-principals")]
    public string[]? AddPrincipals { get; set; }

    [CliOption("--remove-principals")]
    public string[]? RemovePrincipals { get; set; }

    [CliOption("--add-resources")]
    public string[]? AddResources { get; set; }

    [CliOption("--remove-resources")]
    public string[]? RemoveResources { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}