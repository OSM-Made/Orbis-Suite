using OrbisLib2.Common;

namespace OrbisSuiteService.Service
{
    public class DBWatcher
    {
        public delegate void DBChangedHandler();
        public event DBChangedHandler? DBChanged;

        private FileSystemWatcher? _Watcher;

        public DBWatcher()
        {
            _Watcher = new FileSystemWatcher(Config.OrbisPath);

            _Watcher.NotifyFilter = NotifyFilters.Attributes
                         | NotifyFilters.CreationTime
                         | NotifyFilters.DirectoryName
                         | NotifyFilters.FileName
                         | NotifyFilters.LastAccess
                         | NotifyFilters.LastWrite
                         | NotifyFilters.Security
                         | NotifyFilters.Size;

            _Watcher.Changed += OnChanged;

            _Watcher.Filter = Config.DataBaseName;
            _Watcher.IncludeSubdirectories = true;
            _Watcher.EnableRaisingEvents = true;
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (DBChanged != null)
            {
                DBChanged();
            }
        }
    }
}
