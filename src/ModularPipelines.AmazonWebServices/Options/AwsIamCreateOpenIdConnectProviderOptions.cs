using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "create-open-id-connect-provider")]
public record AwsIamCreateOpenIdConnectProviderOptions(
[property: CommandSwitch("--url")] string Url,
[property: CommandSwitch("--thumbprint-list")] string[] ThumbprintList
) : AwsOptions
{
    [CommandSwitch("--client-id-list")]
    public string[]? ClientIdList { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}