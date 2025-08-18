using System;

namespace GameDrive
{
    [Serializable]
    public class Argument<T>
    {
        public string name;
        public T value;

        public Argument(string name, T value)
        {
            this.name = name;
            this.value = value;
        }
    }

}