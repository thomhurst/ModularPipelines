using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kms", "retire-grant")]
public record AwsKmsRetireGrantOptions : AwsOptions
{
    [CommandSwitch("--grant-token")]
    public string? GrantToken { get; set; }

    [CommandSwitch("--key-id")]
    public string? KeyId { get; set; }

    [CommandSwitch("--grant-id")]
    public string? GrantId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}