using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzEventgrid
{
    public AzEventgrid(
        AzEventgridDomain domain,
        AzEventgridEventSubscription eventSubscription,
        AzEventgridExtensionTopic extensionTopic,
        AzEventgridNamespace @namespace,
        AzEventgridPartner partner,
        AzEventgridSystemTopic systemTopic,
        AzEventgridTopic topic,
        AzEventgridTopicType topicType
    )
    {
        Domain = domain;
        EventSubscription = eventSubscription;
        ExtensionTopic = extensionTopic;
        Namespace = @namespace;
        Partner = partner;
        SystemTopic = systemTopic;
        Topic = topic;
        TopicType = topicType;
    }

    public AzEventgridDomain Domain { get; }

    public AzEventgridEventSubscription EventSubscription { get; }

    public AzEventgridExtensionTopic ExtensionTopic { get; }

    public AzEventgridNamespace Namespace { get; }

    public AzEventgridPartner Partner { get; }

    public AzEventgridSystemTopic SystemTopic { get; }

    public AzEventgridTopic Topic { get; }

    public AzEventgridTopicType TopicType { get; }
}