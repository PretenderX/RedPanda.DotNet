using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RedPanda.Service.Governance.Common
{
    public class NewtonsoftJsonProvider : IJsonProvider
    {
        public static void Use()
        {
            ServiceGovernanceConfig.JsonProvider = new NewtonsoftJsonProvider();
        }

        public TValue GetValueFromJson<TValue>(byte[] utf8JsonBytes, string semicolonsJoinedPropertyNames)
        {
            using (var stream = new MemoryStream(utf8JsonBytes))
            using (var streamReader = new StreamReader(stream))
            using (var jsonReader = new JsonTextReader(streamReader))
            {
                var jToken = JToken.Load(jsonReader);
                var returnType = typeof(TValue);
                var properties = semicolonsJoinedPropertyNames.Split(ServiceGovernanceConsts.JsonPropertyNameSplitChar);
                var readDepth = 0;
                var readingElement = jToken.Root;
                var readValueDepth = properties.Length - 1;
                TValue returnValue = default;

                while (readDepth < properties.Length)
                {
                    var parent = readingElement;
                    readingElement = readingElement[properties[readDepth]];

                    if (readingElement == null)
                    {
                        throw new KeyNotFoundException($"未能从{parent}中找到名称为{properties[readDepth]}的属性");
                    }

                    if (readDepth == readValueDepth)
                    {
                        try
                        {
                            if (returnType.IsEnum)
                            {
                                return (TValue)Enum.Parse(returnType, readingElement.Value<string>(), true);
                            }

                            return readingElement.Value<TValue>();
                        }
                        catch (Exception e)
                        {
                            throw new InvalidCastException($"不支持将 \"{readingElement.Value<string>()}\" 转换为 \"{returnType.FullName}\" 类型", e);
                        }
                    }

                    if (readDepth < readValueDepth)
                    {
                        readDepth++;

                        continue;
                    }
                }

                return returnValue;
            }
        }

        public byte[] SerializeToUtf8Bytes<TConfig>(TConfig obj, bool indented = false) where TConfig : new()
        {
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = indented ? Formatting.Indented : Formatting.None
            };
            var jsonString = JsonConvert.SerializeObject(obj, jsonSerializerSettings);

            return Encoding.UTF8.GetBytes(jsonString);
        }

        public TConfig Deserialize<TConfig>(byte[] utf8JsonBytes) where TConfig : new()
        {
            var utf8String = Encoding.UTF8.GetString(utf8JsonBytes);

            return JsonConvert.DeserializeObject<TConfig>(utf8String);
        }

        public string DeserializeUtf8BytesToJson<TConfig>(byte[] utf8JsonBytes, bool indented = false) where TConfig : new()
        {
            var utf8String = Encoding.UTF8.GetString(utf8JsonBytes);
            var obj = JsonConvert.DeserializeObject<TConfig>(utf8String);
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = indented ? Formatting.Indented : Formatting.None
            };

            return JsonConvert.SerializeObject(obj, jsonSerializerSettings);
        }

        public TValue QueryByJsonPath<TValue>(byte[] utf8JsonBytes, string jsonPath)
        {
            using (var stream = new MemoryStream(utf8JsonBytes))
            using (var streamReader = new StreamReader(stream))
            using (var jsonReader = new JsonTextReader(streamReader))
            {
                var jObject = JToken.Load(jsonReader);
                var jToken = jObject.SelectToken(jsonPath);
                var returnType = typeof(TValue);

                if (jToken != null)
                {
                    if (returnType.IsEnum)
                    {
                        return (TValue)Enum.Parse(returnType, jToken.Value<string>(), true);
                    }

                    return jToken.Value<TValue>();
                }

                return default;
            }
        }
    }
}
