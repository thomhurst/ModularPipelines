using ModularPipelines.Events;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Engine.Attributes;
using ModularPipelines.Modules;

using ModularPipelines.Generated;

namespace ModularPipelines.UnitTests.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
internal sealed class GeneratedStartAttribute(string name) : Attribute, IModuleStartHandler
{
    public string Name { get; } = name;

    public int Priority { get; set; }

    public Task OnModuleStartAsync(IModuleHookContext context) => Task.CompletedTask;
}

[AttributeUsage(AttributeTargets.Class, Inherited = true)]
internal sealed class GeneratedMarkerAttribute(string value) : Attribute
{
    public string Value { get; } = value;
}

[GeneratedStart("base", Priority = 20)]
[GeneratedMarker("base-marker")]
public class GeneratedEventBaseModule : Module<string>
{
    protected internal override Task<string> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
        => Task.FromResult<string>("base");
}

[GeneratedStart("derived", Priority = 10)]
[GeneratedStart("repeated", Priority = 30)]
[GeneratedMarker("marker")]
public sealed class GeneratedEventDerivedModule : GeneratedEventBaseModule
{
}

public sealed class GeneratedNoEventModule : Module<string>
{
    protected internal override Task<string> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
        => Task.FromResult<string>("none");
}

public static class GeneratedInaccessibleTypeArgument
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class MarkerAttribute(Type type) : Attribute
    {
        public Type Type { get; } = type;
    }

    [Marker(typeof(InaccessibleType))]
    public sealed class TestModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
            => Task.FromResult<string>("fallback");
    }

    private sealed class InaccessibleType
    {
    }
}

public static class GeneratedNestedInaccessibleTypeArgument
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class MarkerAttribute(Type type) : Attribute
    {
        public Type Type { get; } = type;
    }

    public sealed class Wrapper<T>;

    [Marker(typeof(Wrapper<InaccessibleType[]>))]
    public sealed class TestModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
            => Task.FromResult<string>("fallback");
    }

    private sealed class InaccessibleType;
}

public abstract class GeneratedNamedArgumentBaseAttribute : Attribute
{
    public string? InheritedValue { get; set; }
}

[AttributeUsage(AttributeTargets.Class)]
public sealed class GeneratedInheritedNamedArgumentAttribute : GeneratedNamedArgumentBaseAttribute;

[GeneratedInheritedNamedArgument(InheritedValue = "inherited")]
public sealed class GeneratedInheritedNamedArgumentModule : Module<string>
{
    protected internal override Task<string> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
        => Task.FromResult<string>("generated");
}

public class GeneratedAttributeEventMetadataTests
{
    [Test]
    public async Task Generator_RegistersInheritedRepeatedAndNamedAttributeValues()
    {
        var found = GeneratedModuleEventMetadata.TryCreateAttributes(
            typeof(GeneratedEventDerivedModule),
            out var attributes);

        await Assert.That(found).IsTrue();
        var handlers = attributes.OfType<GeneratedStartAttribute>().ToArray();
        await Assert.That(handlers.Length).IsEqualTo(3);
        await Assert.That(handlers[0].Name).IsEqualTo("derived");
        await Assert.That(handlers[0].Priority).IsEqualTo(10);
        await Assert.That(handlers[1].Name).IsEqualTo("repeated");
        await Assert.That(handlers[1].Priority).IsEqualTo(30);
        await Assert.That(handlers[2].Name).IsEqualTo("base");
        await Assert.That(handlers[2].Priority).IsEqualTo(20);
        await Assert.That(attributes.OfType<GeneratedMarkerAttribute>().Single().Value)
            .IsEqualTo("marker");
    }

    [Test]
    public async Task Generator_RegistersModuleWithoutHooks()
    {
        var found = GeneratedModuleEventMetadata.TryCreateAttributes(
            typeof(GeneratedNoEventModule),
            out var attributes);

        await Assert.That(found).IsTrue();
        await Assert.That(attributes).IsEmpty();
    }

    [Test]
    public async Task Runtime_UsesGeneratedHandlersInPriorityOrder()
    {
        var service = new ModuleAttributeEventService();

        var handlers = service.GetStartHandlers(typeof(GeneratedEventDerivedModule));
        var contextAttributes = service.GetAttributes(typeof(GeneratedEventDerivedModule));

        await Assert.That(((GeneratedStartAttribute) handlers[0]).Name).IsEqualTo("derived");
        await Assert.That(((GeneratedStartAttribute) handlers[1]).Name).IsEqualTo("base");
        await Assert.That(((GeneratedStartAttribute) handlers[2]).Name).IsEqualTo("repeated");
        await Assert.That(ReferenceEquals(
                handlers[0],
                contextAttributes.OfType<GeneratedStartAttribute>().First()))
            .IsTrue();
        await Assert.That(contextAttributes
            .OfType<GeneratedMarkerAttribute>()
            .Single()
            .Value)
            .IsEqualTo("marker");
    }

    [Test]
    public async Task Runtime_ReflectsForInaccessibleDynamicModuleType()
    {
        var generated = GeneratedModuleEventMetadata.TryCreateAttributes(
            typeof(ReflectionFallbackModule),
            out _);
        var service = new ModuleAttributeEventService();

        var handlers = service.GetStartHandlers(typeof(ReflectionFallbackModule));

        await Assert.That(generated).IsFalse();
        await Assert.That(handlers).HasSingleItem();
        await Assert.That(handlers[0]).IsTypeOf<ReflectionFallbackStartAttribute>();
    }

    [Test]
    public async Task Generator_FallsBackForInaccessibleTypeArgument()
    {
        var generated = GeneratedModuleEventMetadata.TryCreateAttributes(
            typeof(GeneratedInaccessibleTypeArgument.TestModule),
            out _);
        var service = new ModuleAttributeEventService();

        var attributes = service.GetAttributes(typeof(GeneratedInaccessibleTypeArgument.TestModule));
        var marker = attributes
            .OfType<GeneratedInaccessibleTypeArgument.MarkerAttribute>()
            .Single();

        await Assert.That(generated).IsFalse();
        await Assert.That(marker.Type.Name).IsEqualTo("InaccessibleType");
    }

    [Test]
    public async Task Generator_FallsBackForNestedInaccessibleTypeArgument()
    {
        var generated = GeneratedModuleEventMetadata.TryCreateAttributes(
            typeof(GeneratedNestedInaccessibleTypeArgument.TestModule),
            out _);
        var service = new ModuleAttributeEventService();

        var attributes = service.GetAttributes(typeof(GeneratedNestedInaccessibleTypeArgument.TestModule));
        var marker = attributes
            .OfType<GeneratedNestedInaccessibleTypeArgument.MarkerAttribute>()
            .Single();

        await Assert.That(generated).IsFalse();
        await Assert.That(marker.Type.Name).IsEqualTo("Wrapper`1");
    }

    [Test]
    public async Task Generator_ResolvesInheritedNamedArgumentMembers()
    {
        var generated = GeneratedModuleEventMetadata.TryCreateAttributes(
            typeof(GeneratedInheritedNamedArgumentModule),
            out var attributes);

        await Assert.That(generated).IsTrue();
        await Assert.That(attributes
                .OfType<GeneratedInheritedNamedArgumentAttribute>()
                .Single()
                .InheritedValue)
            .IsEqualTo("inherited");
    }

    [Test]
    public async Task Registry_KeepsFirstDuplicateRegistration()
    {
        GeneratedModuleEventMetadata.Register(
            typeof(DuplicateRegistrationType),
            static () => [new GeneratedMarkerAttribute("first")]);

        GeneratedModuleEventMetadata.Register(
            typeof(DuplicateRegistrationType),
            static () => [new GeneratedMarkerAttribute("second")]);

        var found = GeneratedModuleEventMetadata.TryCreateAttributes(
            typeof(DuplicateRegistrationType),
            out var attributes);

        await Assert.That(found).IsTrue();
        await Assert.That(attributes.OfType<GeneratedMarkerAttribute>().Single().Value)
            .IsEqualTo("first");
    }

    private sealed class ReflectionFallbackStartAttribute : Attribute, IModuleStartHandler
    {
        public Task OnModuleStartAsync(IModuleHookContext context) => Task.CompletedTask;
    }

    [ReflectionFallbackStart]
    private sealed class ReflectionFallbackModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
            => Task.FromResult<string>("fallback");
    }

    private sealed class DuplicateRegistrationType
    {
    }
}
