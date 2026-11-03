using API_with_DOCKER.interfaces;
using API_with_DOCKER.ThirdPartyPayment;

namespace API_with_DOCKER.DesignsPatterns.Adapter
{
    public class PaymentAdapter:IPaymentService
    {
        private readonly FakePaymentPlatform _sdk;

        public PaymentAdapter(FakePaymentPlatform sdk)
        {
            _sdk = sdk;
        }

        public Task<bool> PayAsync(decimal amount)
        {
            string result =_sdk.ExecuteTransaction(amount, "USD");

            return Task.FromResult(result == "success");
        }
    }
}
