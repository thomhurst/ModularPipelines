using System.Text;
using ModularPipelines.Constants;
using ModularPipelines.Options;

namespace ModularPipelines.Http;

/// <summary>
/// Redacts credentials from URI display text without changing the URI used for requests.
/// </summary>
internal static class HttpUriRedactor
{
    public static string Redact(Uri? uri, HttpLoggingOptions? options = null)
    {
        if (uri is null)
        {
            return string.Empty;
        }

        if (!uri.IsAbsoluteUri
            && uri.OriginalString.StartsWith("//", StringComparison.Ordinal)
            && Uri.TryCreate("https:" + uri.OriginalString, UriKind.Absolute, out var absoluteUri))
        {
            return Redact(absoluteUri, options)["https:".Length..];
        }

        var text = uri.IsAbsoluteUri ? uri.AbsoluteUri : uri.OriginalString;
        if (uri.IsAbsoluteUri && !string.IsNullOrEmpty(uri.UserInfo))
        {
            var schemeDelimiter = text.IndexOf("://", StringComparison.Ordinal);
            var authorityStart = schemeDelimiter >= 0 ? schemeDelimiter + 3 : uri.Scheme.Length + 1;
            var userInfoEnd = text.IndexOf('@', authorityStart);
            if (userInfoEnd >= authorityStart)
            {
                text = text[..authorityStart] + LoggingConstants.SecretMask + text[userInfoEnd..];
            }
        }

        if (uri.IsAbsoluteUri && IsWebhook(uri))
        {
            // Webhook paths are credentials, including URLs used outside the integration helpers.
            return uri.GetComponents(UriComponents.SchemeAndServer, UriFormat.UriEscaped) + "/" + LoggingConstants.SecretMask;
        }

        var fragmentStart = text.IndexOf('#');
        var queryEnd = fragmentStart >= 0 ? fragmentStart : text.Length;
        var queryStart = text.IndexOf('?');
        if (queryStart < 0 || queryStart > queryEnd)
        {
            return text;
        }

        var sensitiveNames = (options ?? HttpLoggingOptions.Default).SensitiveQueryParameterNames;
        var result = new StringBuilder(text[..(queryStart + 1)]);
        var parameters = text[(queryStart + 1)..queryEnd].Split('&');
        for (var index = 0; index < parameters.Length; index++)
        {
            if (index > 0)
            {
                result.Append('&');
            }

            var parameter = parameters[index];
            var equalsIndex = parameter.IndexOf('=');
            if (equalsIndex >= 0 && IsSensitive(parameter[..equalsIndex], sensitiveNames))
            {
                result.Append(parameter.AsSpan(0, equalsIndex + 1));
                result.Append(LoggingConstants.SecretMask);
            }
            else
            {
                result.Append(parameter);
            }
        }

        result.Append(text.AsSpan(queryEnd));
        return result.ToString();
    }

    private static bool IsSensitive(string name, IReadOnlyList<string> sensitiveNames)
    {
        var decodedName = Uri.UnescapeDataString(name.Replace('+', ' '));
        foreach (var sensitiveName in sensitiveNames)
        {
            if (string.Equals(decodedName, sensitiveName, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    private static bool IsWebhook(Uri uri)
    {
        var host = uri.Host.TrimEnd('.');
        return host.Equals("hooks.slack.com", StringComparison.OrdinalIgnoreCase)
               || host.Equals("hooks.slack-gov.com", StringComparison.OrdinalIgnoreCase)
               || host.Equals("webhook.office.com", StringComparison.OrdinalIgnoreCase)
               || host.EndsWith(".webhook.office.com", StringComparison.OrdinalIgnoreCase)
               || ((host.Equals("outlook.office.com", StringComparison.OrdinalIgnoreCase)
                    || host.Equals("outlook.office365.com", StringComparison.OrdinalIgnoreCase))
                   && uri.AbsolutePath.StartsWith("/webhook/", StringComparison.OrdinalIgnoreCase));
    }
}
