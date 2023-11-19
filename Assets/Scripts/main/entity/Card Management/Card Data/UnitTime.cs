using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace main.entity.Card_Management.Card_Data
{
    /// <summary>
    ///     This entity describes the unit used as "action points" / "mana" etc.
    ///     Time is used to play cards and is refilled at the start of a turn.
    /// </summary>
    [Serializable]
    public class UnitTime
    {
        /// <summary>
        ///     The time from a scale of zero to 10 (feel free to change this range)
        /// </summary>
        [FormerlySerializedAs("time")] [Range(0, 10)]
        private int _time;

        /// <summary>
        ///     Yields the time of the unit as an integer
        /// </summary>
        public int Time => _time;
    }
}