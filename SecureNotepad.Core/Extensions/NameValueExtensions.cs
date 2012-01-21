using System;
using System.Collections.Specialized;
using System.Linq;

namespace SecureNotepad.Core.Extensions
{
    public static class NameValueExtensions
    {
        public static NameValueCollection FromHeaderStringToCollection(this string authenticationHeader)
        {
            return authenticationHeader.FromStringToCollection(',');
        }

        public static NameValueCollection FromQueryStringToCollection(this string queryString)
        {
            return queryString.FromStringToCollection('&');
        }

        public static NameValueCollection FromStringToCollection(this string inputString, char delimiter)
        {
            var elements = inputString.Replace("?", string.Empty).Split(delimiter);
            var oauthParameters = new NameValueCollection(elements.Length);
            foreach (var element in elements)
            {
                var keyValues = element.Split(new[] { '=' }, 2);
                oauthParameters.Add(keyValues[0].Trim(), keyValues[1].Replace("\"", string.Empty));
            }

            return oauthParameters;
        }

        public static string FromCollectionToString(this NameValueCollection inputParams, char delimiter, bool uriEncode, bool quoteValue)
        {
            var output = "";
            foreach (var key in inputParams.AllKeys)
            {
                var quoteString = quoteValue ? "\"" : string.Empty;
                output += string.Format("{0}={1}{2}{3}{4}", key, quoteString, uriEncode ? Uri.EscapeDataString(inputParams[key]) : inputParams[key], quoteString, delimiter);
            }

            output = output.TrimEnd(delimiter);

            return output;
        }

        public static string FromCollectionToHeaderString(this NameValueCollection inputParams)
        {
            return inputParams.FromCollectionToString(',', false, true);
        }

        public static string FromCollectionToQueryString(this NameValueCollection inputParams)
        {
            return inputParams.FromCollectionToString('&', true, false);
        }
    }
}
