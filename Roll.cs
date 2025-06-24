namespace RepairTracker
{
    public static class Roll
    {
        public enum RollType
        {
            Guest = 0,
            Tech,
            Owner,
            Admin,
            Client
        }

        public static string CurrentUser { get; set; } = string.Empty;
        public static string UserName { get; set; } = "Guest";
        public static RollType CurrentRoll { get; set; } = RollType.Guest;
    }
}
