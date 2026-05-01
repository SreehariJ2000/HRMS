namespace HRMS.Helpers
{
    public static class DateHelper
    {
        /// <summary>
        /// Calculates the number of business days (excluding weekends) between two dates, inclusive.
        /// If fromDate == toDate and it's a weekday, returns 1.
        /// </summary>
        public static int CalculateBusinessDays(DateTime fromDate, DateTime toDate)
        {
            if (fromDate > toDate)
                return 0;

            int businessDays = 0;
            var currentDate = fromDate.Date;
            var endDate = toDate.Date;

            while (currentDate <= endDate)
            {
                if (currentDate.DayOfWeek != DayOfWeek.Saturday && currentDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    businessDays++;
                }
                currentDate = currentDate.AddDays(1);
            }

            return businessDays;
        }
    }
}
