using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "import-phone-number")]
public record AwsConnectImportPhoneNumberOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--source-phone-number-arn")] string SourcePhoneNumberArn
) : AwsOptions
{
    [CommandSwitch("--phone-number-description")]
    public string? PhoneNumberDescription { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}