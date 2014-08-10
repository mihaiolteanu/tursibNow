using System;

namespace tursibNow.Core
{
    public interface IDateTimeCalculator
    {
        /// <summary>
        /// Returns the current date time
        /// </summary>
         DateTime Now();
    }
}