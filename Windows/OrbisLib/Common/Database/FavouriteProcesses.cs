using SQLite;
using System.Linq.Expressions;

namespace OrbisLib2.Common.Database
{
    public class FavouriteProcesses
    {
        [PrimaryKey, AutoIncrement, NotNull]
        public int Id { get; set; }

        /// <summary>
        /// The priority of which favourite will be automatically attached lower being considered first.
        /// </summary>
        [NotNull, Unique, AutoIncrement]
        public int Priority { get; set; } = 0;

        /// <summary>
        /// The name of the process we favourited.
        /// </summary>
        [NotNull, Unique]
        public string ProcessName { get; set; } = "";

        /// <summary>
        /// Save the current instance into the database.
        /// </summary>
        /// <returns>Returns true if a row was removed from the database.</returns>
        public bool Save()
        {
            if (ProcessName == string.Empty || ProcessName == "")
                return false;

            var db = new SQLiteConnection(Config.DataBasePath);

            // Create the table if it doesn't exist already.
            db.CreateTable<FavouriteProcesses>();

            var result = db.Update(this);
            db.Close();
            return (result > 0);
        }

        /// <summary>
        /// Add the current instance to the database.
        /// </summary>
        /// <returns>Returns true if the row was added to the database.</returns>
        public bool Add()
        {
            try
            {
                if (ProcessName == string.Empty || ProcessName == "")
                    return false;

                var db = new SQLiteConnection(Config.DataBasePath);

                // Create the table if it doesn't exist already.
                db.CreateTable<FavouriteProcesses>();

                var result = db.Insert(this);
                db.Close();
                return (result > 0);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Remove the current instance from the database.
        /// </summary>
        /// <returns>Returns true if the row was removed.</returns>
        public bool Remove()
        {
            var db = new SQLiteConnection(Config.DataBasePath);

            var result = db.Delete(this);
            db.Close();
            return (result > 0);
        }

        /// <summary>
        /// Gets all of the favourite process rows from the database as a list.
        /// </summary>
        /// <returns>All favourites from the database as a list.</returns>
        public List<FavouriteProcesses> AsList()
        {
            var db = new SQLiteConnection(Config.DataBasePath);

            // Create the table if it doesn't exist already.
            db.CreateTable<FavouriteProcesses>();

            var result = db.Table<FavouriteProcesses>().ToList();
            db.Close();
            return result;
        }

        /// <summary>
        /// Find a favourite using a predicate.
        /// </summary>
        /// <param name="predicate">Predicate to specify the paramaters of what to find.</param>
        /// <returns>Returns the favourite if none was found returns null.</returns>
        public static FavouriteProcesses Find(Expression<Func<FavouriteProcesses, bool>> predicate)
        {
            var db = new SQLiteConnection(Config.DataBasePath);

            // Create the table if it doesn't exist already.
            db.CreateTable<FavouriteProcesses>();

            var result = db.Find(predicate);
            db.Close();
            return result;
        }
    }
}
