using System;
using System.Collections.Generic;
using System.Text.Json;

namespace RedPanda.Service.Governance.Common
{
    public class SystemTextJsonProvider : IJsonProvider
    {
        private readonly IDictionary<Type, Func<JsonElement, object>> ReturnValueConvertionMethods;

        public SystemTextJsonProvider()
        {
            ReturnValueConvertionMethods = new Dictionary<Type, Func<JsonElement, object>>();

            AddConvertion<bool>(e => e.GetBoolean());
            AddConvertion<byte>(e => e.GetByte());
            AddConvertion<byte[]>(e => e.GetBytesFromBase64());
            AddConvertion<DateTime>(e => e.GetDateTime());
            AddConvertion<DateTimeOffset>(e => e.GetDateTimeOffset());
            AddConvertion<decimal>(e => e.GetDecimal());
            AddConvertion<double>(e => e.GetDouble());
            AddConvertion<Guid>(e => e.GetGuid());
            AddConvertion<short>(e => e.GetInt16());
            AddConvertion<int>(e => e.GetInt32());
            AddConvertion<long>(e => e.GetInt64());
            AddConvertion<sbyte>(e => e.GetSByte());
            AddConvertion<float>(e => e.GetSingle());
            AddConvertion<string>(e => e.GetString());
            AddConvertion<uint>(e => e.GetUInt32());
            AddConvertion<ulong>(e => e.GetUInt64());
        }

        public TValue GetValueFromJson<TValue>(byte[] utf8JsonBytes, string semicolonsJoinedPropertyNames)
        {
            var doc = JsonDocument.Parse(new ReadOnlyMemory<byte>(utf8JsonBytes, 0, utf8JsonBytes.Length), new JsonDocumentOptions { AllowTrailingCommas = true });
            var properties = semicolonsJoinedPropertyNames.Split(ServiceGovernanceConsts.JsonPropertyNameSplitChar);
            var readDepth = 0;
            var readingElement = doc.RootElement;
            var readValueDepth = properties.Length - 1;
            var returnType = typeof(TValue);
            TValue returnValue = default;

            while (readDepth < properties.Length)
            {
                readingElement = readingElement.GetProperty(properties[readDepth]);

                if (readDepth == readValueDepth)
                {
                    try
                    {
                        if (returnType.IsEnum)
                        {
                            if (readingElement.ValueKind == JsonValueKind.String)
                            {
                                return (TValue)Enum.Parse(returnType, readingElement.GetString(), true);
                            }

                            if (readingElement.ValueKind == JsonValueKind.Number)
                            {
                                return (TValue)Enum.Parse(returnType, readingElement.GetInt32().ToString(), true);
                            }
                        }

                        if (ReturnValueConvertionMethods.TryGetValue(returnType, out var convertion))
                        {
                            return (TValue)Convert.ChangeType(convertion.Invoke(readingElement), returnType);
                        }

                    }
                    catch (Exception e)
                    {
                        throw new InvalidCastException($"不支持将 {readingElement.ValueKind}: \"{readingElement.GetRawText()}\" 转换为 \"{returnType.FullName}\" 类型", e);
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

        public byte[] SerializeToUtf8Bytes<TConfig>(TConfig obj, bool indented = false) where TConfig : new()
        {
            return JsonSerializer.SerializeToUtf8Bytes(obj, new JsonSerializerOptions { IgnoreNullValues = true, WriteIndented = indented });
        }

        public TConfig Deserialize<TConfig>(byte[] utf8JsonBytes) where TConfig : new()
        {
            return JsonSerializer.Deserialize<TConfig>(new ReadOnlySpan<byte>(utf8JsonBytes));
        }

        public string DeserializeUtf8BytesToJson<TConfig>(byte[] utf8JsonBytes, bool indented = false) where TConfig : new()
        {
            var obj = JsonSerializer.Deserialize<TConfig>(new ReadOnlySpan<byte>(utf8JsonBytes));

            return JsonSerializer.Serialize(obj, new JsonSerializerOptions { IgnoreNullValues = true, AllowTrailingCommas = false, WriteIndented = indented });
        }

        private void AddConvertion<T>(Func<JsonElement, object> convertionMethod)
        {
            ReturnValueConvertionMethods.Add(typeof(T), convertionMethod);
        }

        public TValue QueryByJsonPath<TValue>(byte[] utf8JsonBytes, string jsonPath)
        {
            throw new NotSupportedException("JSON Path is not supported by System.Text.Json.");
        }
    }
}
