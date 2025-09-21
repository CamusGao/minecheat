using fNbt;

namespace Minecheat;

public static class Extensions
{
    extension<T>(NbtCompound nbtCompound) where T : NbtTag
    {
        public T GetOrAdd(string tagName, Func<NbtCompound, T> factory)
        {
            var tag = nbtCompound.Get<T>(tagName);
            if (tag == null)
            {
                tag = factory(nbtCompound);
                // ensure the tag has the correct name
                tag.Name = tagName;
                nbtCompound.Add(tag);
            }
            return tag;
        }

        public T GetOrAdd(string tagName, T defaultValue)
        {
            return nbtCompound.GetOrAdd(tagName, _ => defaultValue);
        }
    }
}
