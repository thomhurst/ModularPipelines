using ModularPipelines.Attributes.Events;

namespace ModularPipelines.Engine.Attributes;

/// <summary>
/// Service for discovering and caching attribute event receivers on modules.
/// </summary>
internal interface IModuleAttributeEventService
{
    IReadOnlyList<IModuleRegistrationEventReceiver> GetRegistrationReceivers(Type moduleType);

    IReadOnlyList<IModuleReadyEventReceiver> GetReadyReceivers(Type moduleType);

    IReadOnlyList<IModuleStartEventReceiver> GetStartReceivers(Type moduleType);

    IReadOnlyList<IModuleEndEventReceiver> GetEndReceivers(Type moduleType);

    IReadOnlyList<IModuleFailureEventReceiver> GetFailureReceivers(Type moduleType);

    IReadOnlyList<IModuleSkippedEventReceiver> GetSkippedReceivers(Type moduleType);
}
