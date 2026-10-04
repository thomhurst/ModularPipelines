using System.Text;
using ModularPipelines.Context;

namespace ModularPipelines.UnitTests.Helpers;

public class HexTests
{
    private readonly IHexContext _hex = new Hex();

    [Test]
    [Arguments("")]
    [Arguments("0080ff")]
    [Arguments("466f6f2062617221")]
    public async Task Bytes_Round_Trip(string encoded)
    {
        var bytes = _hex.FromHex(encoded);
        await Assert.That(bytes).IsEquivalentTo(Convert.FromHexString(encoded), TUnit.Assertions.Enums.CollectionOrdering.Matching);
        await Assert.That(_hex.ToHex(bytes)).IsEqualTo(encoded);
    }

    [Test]
    public async Task Text_Overload_Uses_Explicit_Encoding()
    {
        const string text = "Hello, 世界!";
        var encoded = _hex.ToHex(text, Encoding.Unicode);
        await Assert.That(_hex.FromHex(encoded, Encoding.Unicode)).IsEqualTo(text);
    }

    [Test]
    [Arguments("0")]
    [Arguments("0g")]
    [Arguments("00-ff")]
    public async Task Rejects_Invalid_Hex(string encoded)
    {
        await Assert.That(() => _hex.FromHex(encoded)).Throws<FormatException>();
    }
}
