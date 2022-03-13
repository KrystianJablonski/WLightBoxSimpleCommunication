using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceCommunication.Extensions
{
    public static class HttpExtensions
    {
        /// <summary>
        /// Extension to create dictionary with content of <paramref name="metaToken"/> object for post request.
        /// https://geeklearning.io/serialize-an-object-to-an-url-encoded-string-in-csharp/
        /// </summary>
        public static IDictionary<string, string> ToKeyValue(this object metaToken)
        {
            if (metaToken == null)
                return null;

            JToken token = metaToken as JToken;
            if (token == null)
                return ToKeyValue(JObject.FromObject(metaToken));

            if (token.HasValues)
            {
                Dictionary<string, string> contentData = new Dictionary<string, string>();
                foreach (JToken child in token.Children().ToList())
                {
                    IDictionary<string, string> childContent = child.ToKeyValue();
                    if (childContent != null)
                    {
                        contentData = contentData.Concat(childContent)
                                                 .ToDictionary(k => k.Key, v => v.Value);
                    }
                }

                return contentData;
            }

            JValue jValue = token as JValue;
            if (jValue?.Value == null)
            {
                return null;
            }

            string value = jValue?.Type == JTokenType.Date ?
                            jValue?.ToString("o", CultureInfo.InvariantCulture) :
                            jValue?.ToString(CultureInfo.InvariantCulture);

            return new Dictionary<string, string> { { token.Path, value } };
        }
    }
}
