using System.IO;

namespace TeamCityDesktop
{
    /// <summary>
    /// Be nice to the guys at codebetter.com
    /// </summary>
    internal class GenericCache<T> where T : class, new()
    {
        readonly Serializer<T> serializer = new Serializer<T>();

        public GenericCache(string filename = null)
        {
            Filename = filename;
        }

        public string Filename { get; set; }

        public void Save(T data)
        {
            serializer.Save(data, Filename);
        }

        public T Load()
        {
            return File.Exists(Filename) ? serializer.Load(Filename) : new T();
        }
    }
}
