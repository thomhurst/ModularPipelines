using System.Text;

namespace ModularPipelines.Extensions;

internal static class TypeExtensions
{
    public static bool IsOrInheritsFrom(this Type type, Type otherType)
    {
        return type == otherType || type.IsSubclassOf(otherType);
    }
    
    public static string GetRealTypeName(this Type type)
    {
        if (!type.IsGenericType)
        {
            return type.Name;
        }

        var stringBuilder = new StringBuilder();
        
        stringBuilder.Append(type.Name.AsSpan(0, type.Name.IndexOf('`')));
        stringBuilder.Append('<');
        
        var appendComma = false;
        
        foreach (var genericTypeArgument in type.GetGenericArguments())
        {
            if (appendComma)
            {
                stringBuilder.Append(',');
            }

            stringBuilder.Append(GetRealTypeName(genericTypeArgument));
            appendComma = true;
        }
        
        stringBuilder.Append('>');
        
        return stringBuilder.ToString();
    }
}