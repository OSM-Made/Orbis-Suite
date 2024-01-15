using SQLite;
using System.Linq.Expressions;

namespace OrbisLib2.Common.Database
{
    /// <summary>
    /// Information about the targets saved.
    /// </summary>
    [Table("Targets")]
    public class SavedTarget
    {
        [PrimaryKey, AutoIncrement, NotNull]
        public int Id { get; set; }

        /// <summary>
        /// Weather or not this is our default target to be selected on start up.
        /// </summary>
        [NotNull]
        [Column("DefaultTarget")]
        public bool IsDefault { get; set; } = false;

        /// <summary>
        /// The name given to the target.
        /// </summary>
        [NotNull, Unique]
        [Column("TargetName")]
        public string Name { get; set; } = "-";

        /// <summary>
        /// The IP Address as a string.
        /// </summary>
        [NotNull, Unique]
        public string IPAddress { get; set; } = "-";

        /// <summary>
        /// The port used to send payloads to the saved IP Address.
        /// </summary>
        [NotNull]
        public int PayloadPort { get; set; } = 9020;

        private StaticInfo _StaticInfo;
        public StaticInfo StaticInfo
        {
            get
            {
                if (_StaticInfo == null)
                {
                    var db = new SQLiteConnection(Config.DataBasePath);

                    // Create the table if it doesn't exist already.
                    db.CreateTable<StaticInfo>();

                    _StaticInfo = db.Find<StaticInfo>(x => x.TargetId == Id);
                    if (_StaticInfo == null && Id != 0)
                    {
                        _StaticInfo = new StaticInfo();
                        _StaticInfo.TargetId = Id;
                        db.Insert(_StaticInfo);
                        db.Close();
                    }
                    else
                    {
                        db.Close();
                    }
                }

                return _StaticInfo;
            }
        }

        private MutableInfo _MutableInfo;
        public MutableInfo MutableInfo
        {
            get
            {
                if (_MutableInfo == null)
                {
                    var db = new SQLiteConnection(Config.DataBasePath);

                    // Create the table if it doesn't exist already.
                    db.CreateTable<MutableInfo>();

                    _MutableInfo = db.Find<MutableInfo>(x => x.TargetId == Id);
                    if (_MutableInfo == null && Id != 0)
                    {
                        _MutableInfo = new MutableInfo();
                        _MutableInfo.TargetId = Id;
                        db.Insert(_MutableInfo); 
                    }

                    db.Close();
                }

                return _MutableInfo;
            }
        }

        /// <summary>
        /// Remove the default tag from the other row.
        /// </summary>
        private void CheckDefault()
        {
            var defaultTarget = FindDefaultTarget();
            if (IsDefault && defaultTarget != null && defaultTarget.Id != Id)
            {
                defaultTarget.IsDefault = false;
                defaultTarget.Save();
            }
        }

        /// <summary>
        /// Saves the current information about the target to the database.
        /// </summary>
        /// <returns>Returns true if any rows were effected.</returns>
        public bool Save()
        {
            if (Name == string.Empty || Name == "-")
                return false;

            if (IPAddress == string.Empty || IPAddress == "-")
                return false;

            CheckDefault();

            var db = new SQLiteConnection(Config.DataBasePath);

            // Create the table if it doesn't exist already.
            db.CreateTable<SavedTarget>();

            var result = db.Update(this);
            db.Close();
            return (result > 0);
        }

        /// <summary>
        /// Duplicates this class to a new instance.
        /// </summary>
        /// <returns>Returns the new instance.</returns>
        public SavedTarget Clone()
        {
            return (SavedTarget)this.MemberwiseClone();
        }

        /// <summary>
        /// Adds a this Target to the data base.
        /// </summary>
        /// <returns>Returns true if a row was added to the database.</returns>
        public bool Add()
        {
            try
            {
                if (Name == string.Empty || Name == "-")
                    return false;

                if (IPAddress == string.Empty || IPAddress == "-")
                    return false;

                CheckDefault();

                var db = new SQLiteConnection(Config.DataBasePath);

                // Create the table if it doesn't exist already.
                db.CreateTable<SavedTarget>();

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
        /// Removes this current Target from the database.
        /// </summary>
        /// <returns>Returns true if a row was removed from the database.</returns>
        public bool Remove()
        {
            var db = new SQLiteConnection(Config.DataBasePath);

            var result = db.Delete(StaticInfo);
            if (result <= 0)
            {
                Console.WriteLine("Failed to delete child db StaticInfo.");
                return false;
            }

            result = db.Delete(MutableInfo);
            if (result <= 0)
            {
                Console.WriteLine("Failed to delete child db MutableInfo.");
                return false;
            }

            result = db.Delete(this);
            db.Close();
            return (result > 0);
        }

        public static List<SavedTarget> GetTargetList()
        {
            var db = new SQLiteConnection(Config.DataBasePath);

            // Create the table if it doesn't exist already.
            db.CreateTable<SavedTarget>();

            var result = db.Table<SavedTarget>().ToList();
            db.Close();
            return result;
        }

        public static SavedTarget FindDefaultTarget()
        {
            var db = new SQLiteConnection(Config.DataBasePath);

            // Create the table if it doesn't exist already.
            db.CreateTable<SavedTarget>();

            var result = db.Find<SavedTarget>(x => x.IsDefault == true);
            db.Close();
            return result;
        }

        /// <summary>
        /// Find a saved Target by a specific value using a predicate.
        /// </summary>
        /// <param name="predicate">The predicate of the columns we want to match on.</param>
        /// <returns>Returns the first object that matches the predicate.</returns>
        public static SavedTarget FindTarget(Expression<Func<SavedTarget, bool>> predicate)
        {
            var db = new SQLiteConnection(Config.DataBasePath);

            // Create the table if it doesn't exist already.
            db.CreateTable<SavedTarget>();

            var result = db.Find(predicate);
            db.Close();
            return result;
        }

        /// <summary>
        /// Find weahter or not a Target by specific value exists by using a predicate.
        /// </summary>
        /// <param name="predicate">The predicate of the columns we want to match on.</param>
        /// <returns>Returns true if we found a match.</returns>
        public static bool DoesTargetExist(Expression<Func<SavedTarget, bool>> predicate)
        {
            var db = new SQLiteConnection(Config.DataBasePath);

            // Create the table if it doesn't exist already.
            db.CreateTable<SavedTarget>();

            var result = db.Find(predicate);
            db.Close();
            return (result != null);
        }
    }
}

