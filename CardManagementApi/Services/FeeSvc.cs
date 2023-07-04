namespace CardManagementApi
{    
    public class PaymentFeeService
    {
        private static readonly Lazy<PaymentFeeService> lazy = new Lazy<PaymentFeeService>(() => new PaymentFeeService(), LazyThreadSafetyMode.ExecutionAndPublication);
        private decimal fee;

        private PaymentFeeService()
        {
            // Start a new thread to generate random fees every hour
            ThreadPool.QueueUserWorkItem(_ =>
            {
                while (true)
                {
                    // Generate a new random fee
                    var random = new Random();
                    fee = (decimal)random.NextDouble() * 2;

                    // Sleep for an hour
                    Thread.Sleep(TimeSpan.FromHours(1));
                }
            });
        }

        public static PaymentFeeService Instance => lazy.Value;

        public decimal GetCurrentFee()
        {
            return fee;
        }
    }

}
