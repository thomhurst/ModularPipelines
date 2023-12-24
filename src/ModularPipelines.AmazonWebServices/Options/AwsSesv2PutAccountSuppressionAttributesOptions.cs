using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sesv2", "put-account-suppression-attributes")]
public record AwsSesv2PutAccountSuppressionAttributesOptions : AwsOptions
{
    [CommandSwitch("--suppressed-reasons")]
    public string[]? SuppressedReasons { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}