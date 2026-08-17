# Secrets

## Use IOptions<>[​](#use-ioptions "Direct link to Use IOptions<>")

If you don't know how IOptions work, then go check out a tutorial. This is the recommended and supported way of storing configuration in Modular Pipelines.

Your options classes should be registered as IOptions.

If you have any sensitive/secret data stored in these classes, you can attribute your property with `[SecretValue]`.

This attribute, combined with the `IModuleLogger`, means that if that value is ever attempted to be written to logs, it'll be censored out, so that secret values aren't visible to those unauthorised.

`[SecretValue]` supports scalar strings, character sequences such as `char[]`, `IEnumerable<char>`, `Memory<char>`, and `ReadOnlyMemory<char>`, and collections of secret values. Character sequences are treated as one secret, while secret collections are masked item by item.

## Example[​](#example "Direct link to Example")

```
public record MySettings

{

    [SecretValue]

    public string? ApiKey { get; set; }

}
```
