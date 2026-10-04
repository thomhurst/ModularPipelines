using ModularPipelines.Context;
using ModularPipelines.TestHelpers;
using YamlDotNet.Serialization.NamingConventions;
using static ModularPipelines.UnitTests.Helpers.SerializationTestModels;

namespace ModularPipelines.UnitTests.Helpers;

public class YamlTests : TestBase
{
    [Test]
    [Arguments("")]
    [Arguments("null")]
    public async Task Empty_Or_Null_Input_Returns_Null(string input)
    {
        var yaml = await GetService<IYamlContext>();
        await Assert.That(yaml.FromYaml<SerializationTestModel>(input)).IsNull();
        await Assert.That(yaml.FromYaml<SerializationTestModel>(input, CamelCaseNamingConvention.Instance)).IsNull();
    }

    [Test]
    public async Task Can_Serialize_With_Null()
    {
        var yaml = await GetService<IYamlContext>();

        var result = yaml.ToYaml(SerializationTestModel.CreateDefault());
        await Assert.That(result.Trim()).IsEqualTo($"""
                                       foo: {TestValues.FooValue}
                                       hello: {TestValues.HelloValue}
                                       """);
    }

    [Test]
    public async Task Can_Serialize_With_Array()
    {
        var yaml = await GetService<IYamlContext>();

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
        var yaml = await GetService<IYamlContext>();

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
        var yaml = await GetService<IYamlContext>();

        var result = yaml.FromYaml<SerializationTestModel>($"""
                                              foo: {TestValues.FooValue}
                                              hello: {TestValues.HelloValue}
                                              """);
        await Assert.That(result).IsEqualTo(SerializationTestModel.CreateDefault());
    }

    [Test]
    public async Task Can_Deserialize_With_Options()
    {
        var yaml = await GetService<IYamlContext>();

        var result = yaml.FromYaml<SerializationTestModel>($"""
                                              foo: {TestValues.FooValue}
                                              hello: {TestValues.HelloValue}
                                              """, CamelCaseNamingConvention.Instance);
        await Assert.That(result).IsEqualTo(SerializationTestModel.CreateDefault());
    }
}