using System;

namespace AbpTemplate.App.Synchronization.Cache
{
    public readonly struct LockGroup : IEquatable<LockGroup>
    {
        private readonly int _id;
        private readonly string _lockName;

        public LockGroup(int id, string lockName)
        {
            _id = id;
            _lockName = lockName;
        }

        public bool Equals(LockGroup other)
        {
            return _id == other._id && _lockName == other._lockName;
        }

        public override bool Equals(object obj)
        {
            return obj is LockGroup other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_id, _lockName);
        }

        public static bool operator ==(LockGroup left, LockGroup right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(LockGroup left, LockGroup right)
        {
            return !(left == right);
        }
    }
}
