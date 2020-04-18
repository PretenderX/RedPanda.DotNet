using RedPanda.Service.Governance.NewtonsoftJson;

namespace RedPanda.Service.Governance.Test
{
    public class NewtonsoftJsonProviderTests : JsonProviderTestsBase
    {
        public NewtonsoftJsonProviderTests()
            : base(new NewtonsoftJsonProvider())
        {
        }
    }
}
