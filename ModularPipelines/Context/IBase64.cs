using System.Text;

namespace ModularPipelines.Context;

public interface IBase64
{
    string ToBase64String(string input) => ToBase64String(input, Encoding.UTF8);
    string ToBase64String(string input, Encoding encoding);
    string ToBase64String(byte[] bytes);
    
    string FromBase64String(string base64Input) => FromBase64String(base64Input, Encoding.UTF8);
    string FromBase64String(string base64Input, Encoding encoding);
}