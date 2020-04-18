namespace RedPanda.Service.Governance.Common
{
    public interface IJsonProvider
    {
        /// <summary>
        /// 使用":"连接的属性名称从JSON中取值
        /// </summary>
        /// <typeparam name="TValue">返回值类型</typeparam>
        /// <param name="utf8JsonBytes"></param>
        /// <param name="semicolonsJoinedPropertyNames">":"连接的属性名称</param>
        /// <returns></returns>
        TValue GetValueFromJson<TValue>(byte[] utf8JsonBytes, string semicolonsJoinedPropertyNames);

        byte[] SerializeToUtf8Bytes<TConfig>(TConfig obj, bool indented = false) where TConfig : new();

        TConfig Deserialize<TConfig>(byte[] utf8JsonBytes) where TConfig : new();

        string DeserializeUtf8BytesToJson<TConfig>(byte[] utf8JsonBytes, bool indented = false) where TConfig : new();


        /// <summary>
        /// 使用JSON Path从Json中查询值
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="utf8JsonBytes"></param>
        /// <param name="jsonPath">JsonPath (https://goessner.net/articles/JsonPath/ , http://jsonpath.com/)</param>
        /// <returns></returns>
        TValue QueryByJsonPath<TValue>(byte[] utf8JsonBytes, string jsonPath);
    }
}
