using System.IO;
using System.Xml.Serialization;

namespace TeamCityDesktop.ViewModel
{
    public class SerializableViewModel<T> : ViewModelBase
    {
        public static void Save(T obj, string path)
        {
            CreatePath(path);
            var serializer = new XmlSerializer(typeof(T));
            using (var stream = new StreamWriter(path))
            {
                serializer.Serialize(stream, obj);
            }
        }

        public static T Load(string path)
        {
            var deserializer = new XmlSerializer(typeof(T));
            using (var stream = new StreamReader(path))
            {
                object deserialized = deserializer.Deserialize(stream);
                return (T)deserialized;
            }
        }

        private static void CreatePath(string path)
        {
            if (File.Exists(path))
            {
                return;
            }
            string current = "";
            foreach (string part in path.Split(Path.DirectorySeparatorChar))
            {
                current = Path.Combine(current, part);
                if (string.IsNullOrEmpty(Path.GetExtension(part)))
                {
                    if (!Directory.Exists(part))
                    {
                        Directory.CreateDirectory(current);
                    }
                }
            }
        }
    }
}
