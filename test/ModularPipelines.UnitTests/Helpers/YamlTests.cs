using ModularPipelines.Context;
using ModularPipelines.TestHelpers;
using YamlDotNet.Serialization.NamingConventions;
using static ModularPipelines.UnitTests.Helpers.SerializationTestModels;

namespace ModularPipelines.UnitTests.Helpers;

public class YamlTests : TestBase
{
    [Test]
    public async Task Can_Serialize_With_Null()
    {
        var yaml = await GetService<IYaml>();

        var result = yaml.ToYaml(SerializationTestModel.CreateDefault());
        await Assert.That(result.Trim()).IsEqualTo($"""
                                       foo: {TestValues.FooValue}
                                       hello: {TestValues.HelloValue}
                                       """);
    }

    [Test]
    public async Task Can_Serialize_With_Array()
    {
        var yaml = await GetService<IYaml>();

        var result = yaml.ToYaml(SerializationTestModel.CreateWithItems());
        await Assert.That(result.Trim()).IsEqualTo($"""
                                              foo: {TestValues.FooValue}
                                              hello: {TestValues.HelloValue}
                                              items:
                                              - {TestValues.ItemsValue[0]}
                                              - {TestValues.ItemsValue[1]}
                                              - {TestValues.ItemsValue[2]}
                                              """);
    }

    [Test]
    public async Task Can_Serialize_With_Options()
    {
        var yaml = await GetService<IYaml>();

        var result = yaml.ToYaml(SerializationTestModel.CreateDefault(),
            PascalCaseNamingConvention.Instance);
        await Assert.That(result.Trim()).IsEqualTo($"""
                                       Foo: {TestValues.FooValue}
                                       Hello: {TestValues.HelloValue}
                                       """);
    }

    [Test]
    public async Task Can_Deserialize()
    {
        var yaml = await GetService<IYaml>();

        var result = yaml.FromYaml<SerializationTestModel>($"""
                                              foo: {TestValues.FooValue}
                                              hello: {TestValues.HelloValue}
                                              """);
        await Assert.That(result).IsEqualTo(SerializationTestModel.CreateDefault());
    }

    [Test]
    public async Task Can_Deserialize_With_Options()
    {
        var yaml = await GetService<IYaml>();

        var result = yaml.FromYaml<SerializationTestModel>($"""
                                              foo: {TestValues.FooValue}
                                              hello: {TestValues.HelloValue}
                                              """, CamelCaseNamingConvention.Instance);
        await Assert.That(result).IsEqualTo(SerializationTestModel.CreateDefault());
    }
}