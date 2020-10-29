using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTGP7.Common
{
    public class MK7Timer
    {
        private static readonly UInt32 FramesPerSecond = 60;

        public MK7Timer()
        {
            Frames = 0;
        }
        public MK7Timer(UInt32 frames) {
            Frames = frames;
        }
        public MK7Timer(UInt32 minutes, UInt32 seconds, UInt32 milliseconds) {
            Frames = ToFrames(minutes, seconds, milliseconds);
        }
        public MK7Timer(double seconds)
        {
            Frames = (UInt32)(seconds * FramesPerSecond);
        }
        public UInt32 Frames { get; }
        public UInt32 Minutes
        {
            get {
                return (Frames / FramesPerSecond) / 60;
            }
        }
        public UInt32 Seconds
        {
            get
            {
                return (Frames / FramesPerSecond) % 60;
            }
        }
        public double TotalSeconds
        {
            get
            {
                return (Frames / (double)FramesPerSecond);
            }
        }
        public UInt32 Milliseconds
        {
            get
            {
                double tot = ((double)Frames / FramesPerSecond);
                double floor = Math.Floor(tot);
                return (UInt32)(tot * 1000 - floor * 1000);
            }
        }
        public static UInt32 ToFrames(UInt32 minutes, UInt32 seconds, UInt32 milliseconds) {
            return ToFrames(minutes, seconds) + (UInt32)((milliseconds / (double)1000) * FramesPerSecond);
        }
        public static UInt32 ToFrames(UInt32 minutes, UInt32 seconds) {
            return minutes * 60 * FramesPerSecond + seconds * FramesPerSecond;
        }
        public static MK7Timer operator +(MK7Timer left, MK7Timer right)
        {
            return new MK7Timer(left.Frames + right.Frames);
        }
        public static MK7Timer operator -(MK7Timer left, MK7Timer right)
        {
            return new MK7Timer(left.Frames - right.Frames);
        }
        public static MK7Timer operator *(MK7Timer left, UInt32 amount)
        {
            return new MK7Timer(left.Frames * amount);
        }
        public static MK7Timer operator /(MK7Timer left, UInt32 amount)
        {
            return new MK7Timer(left.Frames / amount);
        }
        public static bool operator >(MK7Timer left, MK7Timer right)
        {
            return left.Frames > right.Frames;
        }
        public static bool operator >=(MK7Timer left, MK7Timer right)
        {
            return left.Frames >= right.Frames;
        }
        public static bool operator <(MK7Timer left, MK7Timer right)
        {
            return left.Frames < right.Frames;
        }
        public static bool operator <=(MK7Timer left, MK7Timer right)
        {
            return left.Frames <= right.Frames;
        }

        public override bool Equals(object obj)
        {
            if (obj is MK7Timer)
                return this.Frames == (obj as MK7Timer).Frames;
            else throw new ArgumentException("Object is not MK7Timer");
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public static bool operator ==(MK7Timer left, MK7Timer right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }
            if (ReferenceEquals(left, null))
            {
                return false;
            }
            if (ReferenceEquals(right, null))
            {
                return false;
            }

            return left.Equals(right);
        }
        public static bool operator !=(MK7Timer left, MK7Timer right)
        {
            return !(left == right);
        }
    }
}
