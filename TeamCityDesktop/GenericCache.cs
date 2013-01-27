using System.IO;
using TeamCityDesktop.ViewModel;

namespace TeamCityDesktop
{
    /// <summary>
    /// Be nice to the guys at codebetter.com
    /// </summary>
    internal class GenericCache<T> : SerializableViewModel<T> where T : new()
    {
        private readonly T data;
        private readonly string filename;

        protected GenericCache(string filename)
        {
            this.filename = filename;
        }

        protected GenericCache(string filename, T data)
        {
            this.filename = filename;
            this.data = data;
        }

        public void Save()
        {
            Save(data, filename);
        }

        public T Load()
        {
            return File.Exists(filename) ? Load(filename) : new T();
        }
    }
}
