using System;
namespace Domain
{
    /// <summary>
    /// A model of the actor data.
    /// </summary>
    public class MyData
    {
        /// <summary>
        /// Property A.
        /// </summary>
        public string PropertyA { get; set; } = string.Empty;

        /// <summary>
        /// Property B
        /// </summary>
        public string PropertyB { get; set; } = string.Empty;

        /// <summary>
        /// Converts this instance to a friendly string.
        /// </summary>
        public override string ToString()
        {
            var propAValue = this.PropertyA == null ? "null" : this.PropertyA;
            var propBValue = this.PropertyB == null ? "null" : this.PropertyB;
            return $"PropertyA: {propAValue}, PropertyB: {propBValue}";
        }
    }
}