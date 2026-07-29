namespace ModularPipelines.SourceGenerator.UnitTests;

public class ModuleMetadataGeneratorTests
{
    private const string TestInfrastructure = """
        namespace ModularPipelines.Modules
        {
            public interface IModule;
            public abstract class Module<T> : IModule;
        }

        namespace ModularPipelines.Attributes
        {
            [System.AttributeUsage(
                System.AttributeTargets.Class | System.AttributeTargets.Interface,
                AllowMultiple = true,
                Inherited = true)]
            public class DependsOnAttribute : System.Attribute
            {
                public DependsOnAttribute(System.Type type) => Type = type;
                public System.Type Type { get; }
                public bool Optional { get; set; }
            }

            public class DependsOnAttribute<T> : DependsOnAttribute
            {
                public DependsOnAttribute() : base(typeof(T))
                {
                }
            }
        }
        """;

    [Test]
    public async Task Generates_Registration_And_Inherited_Dependency_Metadata()
    {
        var result = GeneratorTestHarness.Run(new ModuleMetadataGenerator(), TestInfrastructure, """
            namespace Consumer
            {
                public sealed class DependencyModule : ModularPipelines.Modules.Module<string>;

                [ModularPipelines.Attributes.DependsOn<DependencyModule>(Optional = true)]
                public interface IHasDependency;

                public abstract class BaseModule : ModularPipelines.Modules.Module<string>, IHasDependency;

                public sealed class BuildModule : BaseModule;
            }
            """);

        var generated = result.GeneratedTrees.Single().GetText().ToString();

        using (Assert.Multiple())
        {
            await Assert.That(generated)
                .Contains("CreateRegistration<global::Consumer.BuildModule, string>");
            await Assert.That(generated)
                .Contains("new(typeof(global::Consumer.DependencyModule), true)");
            await Assert.That(generated)
                .Contains("CreateRegistration<global::Consumer.DependencyModule, string>");
            await Assert.That(generated).Contains("isComplete: false");
        }
    }

    [Test]
    public async Task Partial_Declarations_Produce_One_Registration()
    {
        var result = GeneratorTestHarness.Run(new ModuleMetadataGenerator(), TestInfrastructure, """
            namespace Consumer
            {
                public sealed partial class BuildModule : ModularPipelines.Modules.Module<string>;
                public sealed partial class BuildModule : ModularPipelines.Modules.Module<string>;
            }
            """);

        var generated = result.GeneratedTrees.Single().GetText().ToString();

        await Assert.That(CountOccurrences(
            generated,
            "CreateRegistration<global::Consumer.BuildModule, string>")).IsEqualTo(1);
    }

    [Test]
    public async Task Partial_Module_Dependency_Metadata_Remains_Incomplete()
    {
        var result = GeneratorTestHarness.Run(new ModuleMetadataGenerator(), TestInfrastructure, """
            namespace Consumer
            {
                public sealed class DependencyModule : ModularPipelines.Modules.Module<string>;

                [ModularPipelines.Attributes.DependsOn<DependencyModule>]
                public sealed partial class BuildModule : ModularPipelines.Modules.Module<string>;
            }
            """);

        var generated = result.GeneratedTrees.Single().GetText().ToString();

        using (Assert.Multiple())
        {
            await Assert.That(generated)
                .Contains("new(typeof(global::Consumer.DependencyModule), false)");
            await Assert.That(generated).Contains("dependenciesComplete: false");
        }
    }

    [Test]
    public async Task Assembly_Metadata_Remains_Incomplete_When_No_Source_Module_Is_Visible()
    {
        var result = GeneratorTestHarness.Run(new ModuleMetadataGenerator(), TestInfrastructure, """
            namespace Consumer
            {
                public static class Marker;
            }
            """);

        var generated = result.GeneratedTrees.Single().GetText().ToString();

        using (Assert.Multiple())
        {
            await Assert.That(generated).DoesNotContain("CreateRegistration<global::Consumer");
            await Assert.That(generated).Contains("isComplete: false");
        }
    }

    [Test]
    public async Task Inaccessible_Module_Uses_Incomplete_Assembly_Metadata()
    {
        var result = GeneratorTestHarness.Run(new ModuleMetadataGenerator(), TestInfrastructure, """
            namespace Consumer
            {
                public sealed class BuildModule : ModularPipelines.Modules.Module<string>;

                public static class Container
                {
                    private sealed class HiddenModule : ModularPipelines.Modules.Module<string>;
                }
            }
            """);

        var generated = result.GeneratedTrees.Single().GetText().ToString();

        using (Assert.Multiple())
        {
            await Assert.That(generated)
                .Contains("CreateRegistration<global::Consumer.BuildModule, string>");
            await Assert.That(generated).DoesNotContain("HiddenModule");
            await Assert.That(generated).Contains("isComplete: false");
            await Assert.That(result.Diagnostics)
                .Contains(diagnostic => diagnostic.Id == "MPG0011"
                                        && diagnostic.GetMessage().Contains("HiddenModule"));
        }
    }

    [Test]
    public async Task Open_Generic_Module_Is_Omitted_With_Assembly_Fallback()
    {
        var result = GeneratorTestHarness.Run(new ModuleMetadataGenerator(), TestInfrastructure, """
            namespace Consumer
            {
                public sealed class BuildModule : ModularPipelines.Modules.Module<string>;
                public sealed class GenericModule<T> : ModularPipelines.Modules.Module<T>;
            }
            """);

        var generated = result.GeneratedTrees.Single().GetText().ToString();

        using (Assert.Multiple())
        {
            await Assert.That(generated).DoesNotContain("GenericModule");
            await Assert.That(generated).Contains("isComplete: false");
        }
    }

    [Test]
    public async Task Closed_Generic_Dependency_Is_Emitted()
    {
        var result = GeneratorTestHarness.Run(new ModuleMetadataGenerator(), TestInfrastructure, """
            namespace Consumer
            {
                public sealed class GenericModule<T> : ModularPipelines.Modules.Module<T>;

                [ModularPipelines.Attributes.DependsOn(typeof(GenericModule<string>))]
                public sealed class BuildModule : ModularPipelines.Modules.Module<string>;
            }
            """);

        var generated = result.GeneratedTrees.Single().GetText().ToString();

        using (Assert.Multiple())
        {
            await Assert.That(generated)
                .Contains("new(typeof(global::Consumer.GenericModule<string>), false)");
            await Assert.That(CountOccurrences(
                generated,
                "CreateRegistration<global::Consumer.GenericModule<string>, string>")).IsEqualTo(1);
            await Assert.That(generated).Contains("dependenciesComplete: true");
        }
    }

    [Test]
    public async Task Direct_IModule_Implementation_Is_Registered()
    {
        var result = GeneratorTestHarness.Run(new ModuleMetadataGenerator(), TestInfrastructure, """
            namespace Consumer
            {
                public sealed class DirectModule : ModularPipelines.Modules.IModule;
            }
            """);

        var generated = result.GeneratedTrees.Single().GetText().ToString();

        await Assert.That(generated)
            .Contains("CreateRegistration<global::Consumer.DirectModule>");
    }

    [Test]
    public async Task Invalid_Legacy_Dependency_Uses_Reflection_Fallback()
    {
        var result = GeneratorTestHarness.Run(new ModuleMetadataGenerator(), TestInfrastructure, """
            namespace Consumer
            {
                [ModularPipelines.Attributes.DependsOn(typeof(string))]
                public sealed class BuildModule : ModularPipelines.Modules.Module<string>;
            }
            """);

        var generated = result.GeneratedTrees.Single().GetText().ToString();

        using (Assert.Multiple())
        {
            await Assert.That(generated).DoesNotContain("new(typeof(string)");
            await Assert.That(generated).Contains("dependenciesComplete: false");
        }
    }

    [Test]
    public async Task Custom_Generic_Dependency_Attribute_Uses_Reflection_Fallback()
    {
        var result = GeneratorTestHarness.Run(new ModuleMetadataGenerator(), TestInfrastructure, """
            namespace Consumer
            {
                public sealed class DependencyModule : ModularPipelines.Modules.Module<string>;

                public sealed class OptionalDependencyAttribute
                    : ModularPipelines.Attributes.DependsOnAttribute<DependencyModule>
                {
                    public OptionalDependencyAttribute()
                    {
                        Optional = true;
                    }
                }

                [OptionalDependency]
                public sealed class BuildModule : ModularPipelines.Modules.Module<string>;
            }
            """);

        var generated = result.GeneratedTrees.Single().GetText().ToString();

        using (Assert.Multiple())
        {
            await Assert.That(generated)
                .DoesNotContain("new(typeof(global::Consumer.DependencyModule)");
            await Assert.That(generated).Contains("dependenciesComplete: false");
        }
    }

    [Test]
    public async Task Unchanged_Compilation_Uses_Incremental_Cache()
    {
        var result = GeneratorTestHarness.RunTwiceWithStepTracking(
            new ModuleMetadataGenerator(),
            TestInfrastructure,
            """
            namespace Consumer
            {
                public sealed class BuildModule : ModularPipelines.Modules.Module<string>;
            }
            """);

        await Assert.That(GeneratorTestHarness.HasCachedOutput(result)).IsTrue();
    }

    private static int CountOccurrences(string source, string value)
    {
        var count = 0;
        var startIndex = 0;
        while ((startIndex = source.IndexOf(value, startIndex, StringComparison.Ordinal)) >= 0)
        {
            count++;
            startIndex += value.Length;
        }

        return count;
    }
}
