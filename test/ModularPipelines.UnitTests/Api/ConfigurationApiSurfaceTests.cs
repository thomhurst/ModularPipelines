using System.Reflection;
using ModularPipelines.Configuration;
using ModularPipelines.Extensions;
using ModularPipelines.Options;

namespace ModularPipelines.UnitTests.Api;

public class ConfigurationApiSurfaceTests
{
    [Test]
    public async Task Pipeline_Options_Are_Sealed()
    {
        await Assert.That(typeof(PipelineOptions).IsSealed).IsTrue();
        await Assert.That(typeof(ConcurrencyOptions).IsSealed).IsTrue();
    }

    [Test]
    public async Task Module_Configuration_Has_No_Public_Setters_Or_Delegates()
    {
        var properties = typeof(ModuleConfiguration).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        await Assert.That(properties.Where(property => property.SetMethod?.IsPublic == true)).IsEmpty();
        await Assert.That(properties.Where(property => typeof(Delegate).IsAssignableFrom(property.PropertyType))).IsEmpty();
        await Assert.That(properties.Select(property => property.Name)).Contains(nameof(ModuleConfiguration.Timeout));
        await Assert.That(properties.Select(property => property.Name)).Contains(nameof(ModuleConfiguration.Tags));
    }

    [Test]
    public async Task Internal_Module_Lookup_Is_Not_Public()
    {
        await Assert.That(typeof(EnumerableExtensions).GetMethod("GetModule", BindingFlags.Public | BindingFlags.Static)).IsNull();
    }

    [Test]
    public async Task Service_Registration_Uses_Builder_Services()
    {
        await Assert.That(typeof(PipelineBuilderExtensions).GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(method => method.Name == "ConfigureServices")).IsEmpty();
    }
}
