using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzProviderhub
{
    public AzProviderhub(
        AzProviderhubCustomRollout customRollout,
        AzProviderhubDefaultRollout defaultRollout,
        AzProviderhubManifest manifest,
        AzProviderhubNotificationRegistration notificationRegistration,
        AzProviderhubOperation operation,
        AzProviderhubProviderRegistration providerRegistration,
        AzProviderhubResourceTypeRegistration resourceTypeRegistration,
        AzProviderhubSku sku
    )
    {
        CustomRollout = customRollout;
        DefaultRollout = defaultRollout;
        Manifest = manifest;
        NotificationRegistration = notificationRegistration;
        Operation = operation;
        ProviderRegistration = providerRegistration;
        ResourceTypeRegistration = resourceTypeRegistration;
        Sku = sku;
    }

    public AzProviderhubCustomRollout CustomRollout { get; }

    public AzProviderhubDefaultRollout DefaultRollout { get; }

    public AzProviderhubManifest Manifest { get; }

    public AzProviderhubNotificationRegistration NotificationRegistration { get; }

    public AzProviderhubOperation Operation { get; }

    public AzProviderhubProviderRegistration ProviderRegistration { get; }

    public AzProviderhubResourceTypeRegistration ResourceTypeRegistration { get; }

    public AzProviderhubSku Sku { get; }
}