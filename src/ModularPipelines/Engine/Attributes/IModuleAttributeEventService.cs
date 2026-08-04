using ModularPipelines.Attributes.Events;

namespace ModularPipelines.Engine.Attributes;

/// <summary>
/// Service for discovering and caching attribute event handlers on modules.
/// </summary>
internal interface IModuleAttributeEventService
{
    IReadOnlyList<Attribute> GetAttributes(Type moduleType);

    IReadOnlyList<IModuleRegistrationEventReceiver> GetRegistrationReceivers(Type moduleType);

    IReadOnlyList<IModuleRegistrationEventReceiver> GetPlanningRegistrationReceivers(Type moduleType);

    IReadOnlyList<IModuleReadyHandler> GetReadyHandlers(Type moduleType);

    IReadOnlyList<IModuleStartHandler> GetStartHandlers(Type moduleType);

    IReadOnlyList<IModuleEndHandler> GetEndHandlers(Type moduleType);

    IReadOnlyList<IModuleFailureHandler> GetFailureHandlers(Type moduleType);

    IReadOnlyList<IModuleSkippedHandler> GetSkippedHandlers(Type moduleType);
}
