using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workmailmessageflow", "get-raw-message-content")]
public record AwsWorkmailmessageflowGetRawMessageContentOptions(
[property: CommandSwitch("--message-id")] string MessageId
) : AwsOptions;