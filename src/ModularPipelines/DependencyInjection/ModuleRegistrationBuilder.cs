using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Options;

namespace ModularPipelines.DependencyInjection;

/// <summary>
/// Fluent builder for configuring module registration with metadata.
/// Uses IServiceCollection.Configure to store metadata for later application.
/// </summary>
internal sealed class ModuleRegistrationBuilder : IModuleRegistrationBuilder
{
    private readonly Type _moduleType;

    public ModuleRegistrationBuilder(IServiceCollection services, Type moduleType)
    {
        Services = services;
        _moduleType = moduleType;
    }

    /// <inheritdoc />
    public IServiceCollection Services { get; }

    /// <inheritdoc />
    public IModuleRegistrationBuilder WithTags(params string[] tags)
    {
        Services.Configure<ModuleRegistrationOptions>(options => options.AddTags(_moduleType, tags));
        return this;
    }

    /// <inheritdoc />
    public IModuleRegistrationBuilder WithCategory(string category)
    {
        Services.Configure<ModuleRegistrationOptions>(options => options.SetCategory(_moduleType, category));
        return this;
    }
}
