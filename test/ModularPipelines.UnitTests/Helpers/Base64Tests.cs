using System.Text;
using ModularPipelines.Context;

namespace ModularPipelines.UnitTests.Helpers;

public class Base64Tests
{
    private readonly IBase64Context _base64 = new Base64();

    [Test]
    [Arguments("", "")]
    [Arguments("é😀", "w6nwn5iA")]
    public async Task Text_Overload_Defaults_To_Utf8(string text, string expected)
    {
        await Assert.That(_base64.ToBase64String(text)).IsEqualTo(expected);
    }

    [Test]
    [Arguments("", "")]
    [Arguments("é😀", "6QA92ADe")]
    public async Task Text_Overload_Uses_Explicit_Encoding(string text, string expected)
    {
        await Assert.That(_base64.ToBase64String(text, Encoding.Unicode)).IsEqualTo(expected);
    }

    [Test]
    public async Task Bytes_Preserve_Binary_Data()
    {
        byte[] bytes = [0x00, 0x80, 0xff, 0x01, 0x7f];
        const string expected = "AID/AX8=";

        var encoded = _base64.ToBase64String(bytes);

        await Assert.That(encoded).IsEqualTo(expected);
        await Assert.That(_base64.FromBase64String(expected)).IsEquivalentTo(bytes, TUnit.Assertions.Enums.CollectionOrdering.Matching);
        await Assert.That(_base64.FromBase64String(encoded)).IsEquivalentTo(bytes, TUnit.Assertions.Enums.CollectionOrdering.Matching);
    }

    [Test]
    public async Task Empty_Bytes_Round_Trip()
    {
        await Assert.That(_base64.ToBase64String(Array.Empty<byte>())).IsEqualTo(string.Empty);
        await Assert.That(_base64.FromBase64String(string.Empty)).IsEmpty();
    }

    [Test]
    [Arguments(" \tA I\r\nD/ A X8=\n ", "0080ff017f")]
    [Arguments(" \t\r\n", "")]
    public async Task Decode_Ignores_Base64_Whitespace(string encoded, string expectedHex)
    {
        await Assert.That(_base64.FromBase64String(encoded)).IsEquivalentTo(
            Convert.FromHexString(expectedHex), TUnit.Assertions.Enums.CollectionOrdering.Matching);
    }

    [Test]
    [Arguments("A")]
    [Arguments("A===")]
    [Arguments("AA=A")]
    [Arguments("AID_")]
    [Arguments("AID/\u00a0")]
    public async Task Rejects_Invalid_Base64(string encoded)
    {
        await Assert.That(() => _base64.FromBase64String(encoded)).Throws<FormatException>();
    }
}
