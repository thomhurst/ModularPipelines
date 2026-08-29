using ModularPipelines.Events;

namespace ModularPipelines.Engine.Attributes;

/// <summary>
/// Service for discovering and caching attribute event handlers on modules.
/// </summary>
internal interface IModuleAttributeEventService
{
    IReadOnlyList<Attribute> GetAttributes(Type moduleType);

    IReadOnlyList<IModuleRegistrationHandler> GetRegistrationHandlers(Type moduleType);

    IReadOnlyList<IModuleRegistrationHandler> GetPlanningRegistrationHandlers(Type moduleType);

    IReadOnlyList<Attribute> GetPlanningAttributes(Type moduleType);

    IReadOnlyList<IModuleReadyHandler> GetReadyHandlers(Type moduleType);

    IReadOnlyList<IModuleStartHandler> GetStartHandlers(Type moduleType);

    IReadOnlyList<IModuleEndHandler> GetEndHandlers(Type moduleType);

    IReadOnlyList<IModuleFailureHandler> GetFailureHandlers(Type moduleType);

    IReadOnlyList<IModuleSkippedHandler> GetSkippedHandlers(Type moduleType);
}
