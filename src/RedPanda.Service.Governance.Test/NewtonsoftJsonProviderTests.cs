using RedPanda.Service.Governance.Common;

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
