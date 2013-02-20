using System.IO;

namespace TeamCityDesktop
{
    /// <summary>
    /// Be nice to the guys at codebetter.com
    /// </summary>
    internal class GenericCache<T> where T : class, new()
    {
        public GenericCache(string filename = null)
        {
            Filename = filename;
        }

        public string Filename { get; set; }

        public void Save(T data)
        {
            Serializer<T>.Save(data, Filename);
        }

        public T Load()
        {
            return File.Exists(Filename) ? Serializer<T>.Load(Filename) : new T();
        }
    }
}
