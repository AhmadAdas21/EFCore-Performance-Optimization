namespace API_with_DOCKER.ThirdPartyPayment
{
    public class FakePaymentPlatform
    {
        public string ExecuteTransaction(decimal amount, string currency)
        {
            return amount > 0 ? "success" : "failed";
        }
    }
}
