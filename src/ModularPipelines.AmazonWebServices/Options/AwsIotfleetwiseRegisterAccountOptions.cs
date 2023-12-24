using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotfleetwise", "register-account")]
public record AwsIotfleetwiseRegisterAccountOptions : AwsOptions
{
    [CommandSwitch("--timestream-resources")]
    public string? TimestreamResources { get; set; }

    [CommandSwitch("--iam-resources")]
    public string? IamResources { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}