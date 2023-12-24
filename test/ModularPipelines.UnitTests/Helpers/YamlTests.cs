using ModularPipelines.Context;
using ModularPipelines.TestHelpers;
using YamlDotNet.Serialization.NamingConventions;

namespace ModularPipelines.UnitTests.Helpers;

public class YamlTests : TestBase
{
    [Test]
    public async Task Can_Serialize_With_Null()
    {
        var yaml = await GetService<IYaml>();

        var result = yaml.ToYaml(new YamlModel { Foo = "Bar!", Hello = "World!" });

        Assert.That(result.Trim(), Is.EqualTo("""
                                       foo: Bar!
                                       hello: World!
                                       """));
    }

    [Test]
    public async Task Can_Serialize_With_Array()
    {
        var yaml = await GetService<IYaml>();

        var result = yaml.ToYaml(new YamlModel
        {
            Foo = "Bar!",
            Hello = "World!",
            Items = new List<string> { "One", "Two", "3" },
        });

        Assert.That(result.Trim(), Is.EqualTo("""
                                              foo: Bar!
                                              hello: World!
                                              items:
                                              - One
                                              - Two
                                              - 3
                                              """));
    }

    [Test]
    public async Task Can_Serialize_With_Options()
    {
        var yaml = await GetService<IYaml>();

        var result = yaml.ToYaml(new YamlModel { Foo = "Bar!", Hello = "World!" },
            PascalCaseNamingConvention.Instance);

        Assert.That(result.Trim(), Is.EqualTo("""
                                       Foo: Bar!
                                       Hello: World!
                                       """));
    }

    [Test]
    public async Task Can_Deserialize()
    {
        var yaml = await GetService<IYaml>();

        var result = yaml.FromYaml<YamlModel>("""
                                              foo: Bar!
                                              hello: World!
                                              """);

        Assert.That(result, Is.EqualTo(new YamlModel { Foo = "Bar!", Hello = "World!" }));
    }

    [Test]
    public async Task Can_Deserialize_With_Options()
    {
        var yaml = await GetService<IYaml>();

        var result = yaml.FromYaml<YamlModel>("""
                                              foo: Bar!
                                              hello: World!
                                              """, CamelCaseNamingConvention.Instance);

        Assert.That(result, Is.EqualTo(new YamlModel { Foo = "Bar!", Hello = "World!" }));
    }

    private record YamlModel
    {
        public string? Foo { get; set; }

        public string? Hello { get; set; }

        public List<string>? Items { get; set; }
    }
}