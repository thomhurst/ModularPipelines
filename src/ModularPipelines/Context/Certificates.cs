using System.Security.Cryptography.X509Certificates;

namespace ModularPipelines.Context;

internal class Certificates : ICertificatesContext
{
    public X509Certificate2? GetCertificateBySubject(StoreLocation storeLocation, string subject)
    {
        return GetCertificateBySubject(StoreName.My, storeLocation, subject, validOnly: false, exactMatch: false);
    }

    public X509Certificate2? GetCertificateBySubject(StoreName storeName, StoreLocation storeLocation, string subject, bool validOnly = true, bool exactMatch = true)
    {
        var findType = exactMatch ? X509FindType.FindBySubjectDistinguishedName : X509FindType.FindBySubjectName;
        return GetCertificateBy(storeName, storeLocation, findType, subject, validOnly);
    }

    public X509Certificate2? GetCertificateByThumbprint(StoreLocation storeLocation, string thumbprint)
    {
        return GetCertificateByThumbprint(StoreName.My, storeLocation, thumbprint, validOnly: false);
    }

    public X509Certificate2? GetCertificateByThumbprint(StoreName storeName, StoreLocation storeLocation, string thumbprint, bool validOnly = true)
    {
        return GetCertificateBy(storeName, storeLocation, X509FindType.FindByThumbprint, thumbprint, validOnly);
    }

    public X509Certificate2? GetCertificateBySerialNumber(StoreLocation storeLocation, string serialNumber)
    {
        return GetCertificateBySerialNumber(StoreName.My, storeLocation, serialNumber, validOnly: false);
    }

    public X509Certificate2? GetCertificateBySerialNumber(StoreName storeName, StoreLocation storeLocation, string serialNumber, bool validOnly = true)
    {
        return GetCertificateBy(storeName, storeLocation, X509FindType.FindBySerialNumber, serialNumber, validOnly);
    }

    public X509Certificate2? GetCertificateBy(StoreLocation storeLocation, X509FindType findType, string findValue)
    {
        return GetCertificateBy(StoreName.My, storeLocation, findType, findValue, validOnly: false);
    }

    public X509Certificate2? GetCertificateBy(StoreName storeName, StoreLocation storeLocation, X509FindType findType, string findValue, bool validOnly = true)
    {
        using var store = new X509Store(storeName, storeLocation);
        store.Open(OpenFlags.ReadOnly);

        return FindCertificate(store.Certificates, findType, findValue, validOnly);
    }

    internal static X509Certificate2? FindCertificate(X509Certificate2Collection certificates, X509FindType findType, string findValue, bool validOnly)
    {
        X509Certificate2Collection? matches = null;
        try
        {
            matches = certificates.Find(findType, findValue, validOnly);
            // Copy the selected certificate so it (and its private key) outlives both collections.
            return matches.Count == 0 ? null : new X509Certificate2(matches[0]);
        }
        finally
        {
            if (matches is not null)
            {
                foreach (var certificate in matches)
                {
                    certificate.Dispose();
                }
            }

            foreach (var certificate in certificates)
            {
                certificate.Dispose();
            }
        }
    }
}
