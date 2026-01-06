namespace MiniProjects.FraudulentOrders
{
    class FraudulentOrders
    {
        static readonly string[] OrderIds = [
            "B123",
            "C234",
            "A345",
            "C15",
            "B177",
            "G3003",
            "C235",
            "B179"
        ];

        public static void Detect()
        {
            foreach (string id in OrderIds)
            {
                if (id.StartsWith('B'))
                {
                    Console.WriteLine(id);
                }
            }
        }
    }
}