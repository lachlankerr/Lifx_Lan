using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Lifx_Lan.Packets.Payloads
{
    /// <summary>
    /// This packet provides information about the device.
    /// 
    /// This packet is the reply to the GetInfo (34) message
    /// </summary>
    internal class StateInfo : Payload, IReceivable
    {
        /// <summary>
        /// The current time according to the device. 
        /// Note that this is most likely inaccurate.
        /// </summary>
        public ulong Time { get; } = 0;

        /// <summary>
        /// The amount of time in nanoseconds the device has been online since last power on
        /// </summary>
        public ulong Uptime { get; } = 0;

        /// <summary>
        /// The amount of time in nanseconds of power off time accurate to 5 seconds.
        /// </summary>
        public ulong Downtime { get; } = 0;

        /// <summary>
        /// Creates an instance of the <see cref="StateInfo"/> class so we can see the values received from the packet
        /// </summary>
        /// <param name="bytes">The payload data from the received <see cref="StateInfo"/> packet</param>
        /// <exception cref="ArgumentException"></exception>
        public StateInfo(byte[] bytes) : base(bytes) 
        {
            if (bytes.Length != 24)
                throw new ArgumentException("Wrong number of bytes for this payload type, expected 24");

            Time = BitConverter.ToUInt64(bytes, 0); 
            Uptime = BitConverter.ToUInt64(bytes, 8);
            Downtime = BitConverter.ToUInt64(bytes, 16);
        }

        private string HumanReadableTimespan(TimeSpan ts)
        {
            string timeString = "";
            if (ts.TotalDays >= 1) { timeString += $" {ts.Days} day(s),"; }
            if (ts.Hours >= 1) { timeString += $" {ts.Hours} hour(s),"; }
            if (ts.Minutes >= 1) { timeString += $" {ts.Minutes} minute(s),"; }
            if (ts.Seconds >= 0) { timeString += $" {ts.Seconds} second(s)"; }
            return timeString;
        }

        public override string ToString()
        {
            TimeSpan uptime = new TimeSpan((long)(Uptime / 100));
            TimeSpan downtime = new TimeSpan((long)(Downtime / 100));

            return $@"Time: {Time} nanoseconds
Uptime:{HumanReadableTimespan(uptime)}
Downtime:{HumanReadableTimespan(downtime)}";
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType()))
                return false;
            else
            {
                StateInfo stateInfo = (StateInfo)obj;
                return Time == stateInfo.Time &&
                       Uptime == stateInfo.Uptime &&
                       Downtime ==stateInfo.Downtime;
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Time, Uptime, Downtime);
        }
    }
}
