using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicebus", "topic", "subscription", "rule", "create")]
public record AzServicebusTopicSubscriptionRuleCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--namespace-name")] string NamespaceName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--subscription-name")] string SubscriptionName,
[property: CommandSwitch("--topic-name")] string TopicName
) : AzOptions
{
    [CommandSwitch("--action-compatibility-level")]
    public string? ActionCompatibilityLevel { get; set; }

    [CommandSwitch("--action-sql-expression")]
    public string? ActionSqlExpression { get; set; }

    [CommandSwitch("--content-type")]
    public string? ContentType { get; set; }

    [CommandSwitch("--correlation-filter")]
    public string? CorrelationFilter { get; set; }

    [CommandSwitch("--correlation-id")]
    public string? CorrelationId { get; set; }

    [BooleanCommandSwitch("--enable-action-preprocessing")]
    public bool? EnableActionPreprocessing { get; set; }

    [BooleanCommandSwitch("--enable-correlation-preprocessing")]
    public bool? EnableCorrelationPreprocessing { get; set; }

    [BooleanCommandSwitch("--enable-sql-preprocessing")]
    public bool? EnableSqlPreprocessing { get; set; }

    [CommandSwitch("--filter-sql-expression")]
    public string? FilterSqlExpression { get; set; }

    [CommandSwitch("--filter-type")]
    public string? FilterType { get; set; }

    [CommandSwitch("--label")]
    public string? Label { get; set; }

    [CommandSwitch("--message-id")]
    public string? MessageId { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--reply-to")]
    public string? ReplyTo { get; set; }

    [CommandSwitch("--reply-to-session-id")]
    public string? ReplyToSessionId { get; set; }

    [CommandSwitch("--session-id")]
    public string? SessionId { get; set; }

    [CommandSwitch("--to")]
    public string? To { get; set; }
}