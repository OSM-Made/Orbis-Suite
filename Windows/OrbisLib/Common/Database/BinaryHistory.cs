using SQLite;
using System.Linq.Expressions;

namespace OrbisLib2.Common.Database
{
    public class BinaryHistory
    {
        [PrimaryKey, AutoIncrement, NotNull]
        public int Id { get; set; }

        [NotNull]
        public bool IsRemote { get; set; } = false;

        [NotNull, Unique]
        public string Path { get; set; } = "";

        [NotNull]
        public string TitleId { get; set; } = "";

        [NotNull]
        public DateTime LastLoaded { get; set; }

        /// <summary>
        /// Save the current instance into the database.
        /// </summary>
        /// <returns>Returns true if a row was removed from the database.</returns>
        public bool Save()
        {
            if (Path == string.Empty || Path == "")
                return false;

            var db = new SQLiteConnection(Config.DataBasePath);

            // Create the table if it doesn't exist already.
            db.CreateTable<BinaryHistory>();

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
                if (Path == string.Empty || Path == "")
                    return false;

                var db = new SQLiteConnection(Config.DataBasePath);

                // Create the table if it doesn't exist already.
                db.CreateTable<BinaryHistory>();

                // Insert the new entry.
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
        public static List<BinaryHistory> AsList()
        {
            var db = new SQLiteConnection(Config.DataBasePath);

            // Create the table if it doesn't exist already.
            db.CreateTable<BinaryHistory>();

            var result = db.Table<BinaryHistory>().OrderByDescending(x => x.LastLoaded).ToList();
            db.Close();
            return result;
        }

        /// <summary>
        /// Find a favourite using a predicate.
        /// </summary>
        /// <param name="predicate">Predicate to specify the paramaters of what to find.</param>
        /// <returns>Returns the favourite if none was found returns null.</returns>
        public static BinaryHistory Find(Expression<Func<BinaryHistory, bool>> predicate)
        {
            var db = new SQLiteConnection(Config.DataBasePath);

            // Create the table if it doesn't exist already.
            db.CreateTable<BinaryHistory>();

            var result = db.Find(predicate);
            db.Close();
            return result;
        }
    }
}
