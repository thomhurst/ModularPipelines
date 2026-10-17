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
        var xml = await GetService<IXmlContext>();

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
        var xml = await GetService<IXmlContext>();

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
    public async Task Can_Serialize_With_DisableFormatting()
    {
        var xml = await GetService<IXmlContext>();

        var result = xml.ToXml(XmlModel.CreateWithItems(), SaveOptions.DisableFormatting);
        await Assert.That(result).IsEqualTo(
            "<XmlModel xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">"
            + $"<Foo>{TestValues.FooValue}</Foo><Hello>{TestValues.HelloValue}</Hello><Items>"
            + $"<string>{TestValues.ItemsValue[0]}</string><string>{TestValues.ItemsValue[1]}</string><string>{TestValues.ItemsValue[2]}</string>"
            + "</Items></XmlModel>");
    }

    [Test]
    [Arguments(SaveOptions.None)]
    [Arguments(SaveOptions.DisableFormatting)]
    public async Task Can_Round_Trip_With_Options(SaveOptions options)
    {
        var xml = await GetService<IXmlContext>();
        var model = XmlModel.CreateWithItems();

        var result = xml.FromXml<XmlModel>(xml.ToXml(model, options));

        await Assert.That(result).IsNotNull();
        await Assert.That(result!.Foo).IsEqualTo(model.Foo);
        await Assert.That(result.Hello).IsEqualTo(model.Hello);
        await Assert.That(result.Items).IsEquivalentTo(model.Items!, TUnit.Assertions.Enums.CollectionOrdering.Matching);
    }

    [Test]
    public async Task Can_Deserialize()
    {
        var xml = await GetService<IXmlContext>();

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
        var xml = await GetService<IXmlContext>();

        var result = xml.FromXml<XmlModel>($"""
                                              <XmlModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
                                                <Foo>{TestValues.FooValue}</Foo>
                                                <Hello>{TestValues.HelloValue}</Hello>
                                              </XmlModel>
                                              """, LoadOptions.None);
        await Assert.That(result).IsEqualTo(XmlModel.CreateDefault());
    }
}
