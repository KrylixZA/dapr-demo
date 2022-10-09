using System;
using Dapr.Actors;

namespace Domain
{
    /// <summary>
    /// Defines a contract for my test actor.
    /// </summary>
    public interface IMyActor : IActor
    {
        /// <summary>
        /// Sets the data.
        /// </summary>
        /// <param name="data">The data.</param>
        Task<string> SetDataAsync(MyData data);

        /// <summary>
        /// Gets the data.
        /// </summary>
        Task<MyData> GetDataAsync();

        /// <summary>
        /// Registers a reminder.
        /// </summary>
        Task RegisterReminder();

        /// <summary>
        /// Unregisters a reminder.
        /// </summary>
        Task UnregisterReminder();

        /// <summary>
        /// Registers a timer.
        /// </summary>
        Task RegisterTimer();

        /// <summary>
        /// Unregisters a timer.
        /// </summary>
        Task UnregisterTimer();
    }
}