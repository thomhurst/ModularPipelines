using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicebus", "topic", "subscription", "rule", "update")]
public record AzServicebusTopicSubscriptionRuleUpdateOptions : AzOptions
{
    [CommandSwitch("--action-compatibility-level")]
    public string? ActionCompatibilityLevel { get; set; }

    [BooleanCommandSwitch("--action-preprocessing")]
    public bool? ActionPreprocessing { get; set; }

    [CommandSwitch("--action-sql-expression")]
    public string? ActionSqlExpression { get; set; }

    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--compatibility-level")]
    public string? CompatibilityLevel { get; set; }

    [CommandSwitch("--content-type")]
    public string? ContentType { get; set; }

    [CommandSwitch("--correlation-id")]
    public string? CorrelationId { get; set; }

    [BooleanCommandSwitch("--enable-correlation-preprocessing")]
    public bool? EnableCorrelationPreprocessing { get; set; }

    [BooleanCommandSwitch("--enable-sql-preprocessing")]
    public bool? EnableSqlPreprocessing { get; set; }

    [CommandSwitch("--filter-sql-expression")]
    public string? FilterSqlExpression { get; set; }

    [CommandSwitch("--filter-type")]
    public string? FilterType { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--label")]
    public string? Label { get; set; }

    [CommandSwitch("--message-id")]
    public string? MessageId { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--namespace-name")]
    public string? NamespaceName { get; set; }

    [CommandSwitch("--properties")]
    public string? Properties { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--reply-to")]
    public string? ReplyTo { get; set; }

    [CommandSwitch("--reply-to-session-id")]
    public string? ReplyToSessionId { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--session-id")]
    public string? SessionId { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--subscription-name")]
    public string? SubscriptionName { get; set; }

    [CommandSwitch("--to")]
    public string? To { get; set; }

    [CommandSwitch("--topic-name")]
    public string? TopicName { get; set; }
}