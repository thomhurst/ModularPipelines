using System.Xml.Linq;
using ModularPipelines.Context;
using ModularPipelines.TestHelpers;
using static ModularPipelines.UnitTests.Helpers.SerializationTestModels;

namespace ModularPipelines.UnitTests.Helpers;

public class XmlTests : TestBase
{
    // XML serializer requires a concrete type, and the model name affects the XML output
    // Keep XmlModel as a separate record that mirrors SerializationTestModel
    public record XmlModel
    {
        public string? Foo { get; set; }
        public string? Hello { get; set; }
        public string[]? Items { get; set; }

        public static XmlModel CreateDefault() =>
            new() { Foo = TestValues.FooValue, Hello = TestValues.HelloValue };

        public static XmlModel CreateWithItems() =>
            new()
            {
                Foo = TestValues.FooValue,
                Hello = TestValues.HelloValue,
                Items = TestValues.ItemsValue,
            };
    }

    [Test]
    public async Task Can_Serialize_With_Null()
    {
        var xml = await GetService<IXml>();

        var result = xml.ToXml(XmlModel.CreateDefault());
        await Assert.That(result.Trim()).IsEqualTo($"""
                                       <XmlModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
                                         <Foo>{TestValues.FooValue}</Foo>
                                         <Hello>{TestValues.HelloValue}</Hello>
                                       </XmlModel>
                                       """);
    }

    [Test]
    public async Task Can_Serialize_With_Array()
    {
        var xml = await GetService<IXml>();

        var result = xml.ToXml(XmlModel.CreateWithItems());
        await Assert.That(result.Trim()).IsEqualTo($"""
                                              <XmlModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
                                                <Foo>{TestValues.FooValue}</Foo>
                                                <Hello>{TestValues.HelloValue}</Hello>
                                                <Items>
                                                  <string>{TestValues.ItemsValue[0]}</string>
                                                  <string>{TestValues.ItemsValue[1]}</string>
                                                  <string>{TestValues.ItemsValue[2]}</string>
                                                </Items>
                                              </XmlModel>
                                              """);
    }

    [Test]
    public async Task Can_Serialize_With_Options()
    {
        var xml = await GetService<IXml>();

        var result = xml.ToXml(XmlModel.CreateDefault(), SaveOptions.DisableFormatting);
        await Assert.That(result.Trim()).IsEqualTo($"""
                                       <XmlModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
                                         <Foo>{TestValues.FooValue}</Foo>
                                         <Hello>{TestValues.HelloValue}</Hello>
                                       </XmlModel>
                                       """);
    }

    [Test]
    public async Task Can_Deserialize()
    {
        var xml = await GetService<IXml>();

        var result = xml.FromXml<XmlModel>($"""
                                              <XmlModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
                                                <Foo>{TestValues.FooValue}</Foo>
                                                <Hello>{TestValues.HelloValue}</Hello>
                                              </XmlModel>
                                              """);
        await Assert.That(result).IsEqualTo(XmlModel.CreateDefault());
    }

    [Test]
    public async Task Can_Deserialize_With_Options()
    {
        var xml = await GetService<IXml>();

        var result = xml.FromXml<XmlModel>($"""
                                              <XmlModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
                                                <Foo>{TestValues.FooValue}</Foo>
                                                <Hello>{TestValues.HelloValue}</Hello>
                                              </XmlModel>
                                              """, LoadOptions.None);
        await Assert.That(result).IsEqualTo(XmlModel.CreateDefault());
    }
}