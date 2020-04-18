using RedPanda.Service.Governance.Common;

namespace RedPanda.Service.Governance.Test
{
    public class SystemTextJsonProviderTests : JsonProviderTestsBase
    {
        public SystemTextJsonProviderTests()
            :base(new SystemTextJsonProvider())
        {
        }

        /// <summary>
        /// Skip JSON Path test for SystemTextJsonProvider
        /// </summary>
        public override void QueryByJsonPath_Should_ReturnValueAccordingToGenericValueTypeAndJsonPathPassedIn()
        {
        }
    }
}
