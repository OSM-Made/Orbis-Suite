using OrbisLib2.Common.Database;
using OrbisLib2.General;
using System.Linq.Expressions;

namespace OrbisLib2.Targets
{
    public class TargetManager
    {
        /// <summary>
        /// Returns a list of the Targets saved.
        /// </summary>
        public static List<Target> Targets
        {
            get
            {
                var temporaryList = new List<Target>();
                var savedTargets = SavedTarget.GetTargetList();

                if (savedTargets == null)
                    return temporaryList;

                foreach (var target in savedTargets)
                {
                    temporaryList.Add(new Target(target));
                }

                return temporaryList;
            }
        }

        private static Target _SelectedTarget;

        /// <summary>
        /// Gets the currently selected target.
        /// </summary>
        public static Target SelectedTarget
        {
            get
            {
                // Set initially as the default target.
                if (_SelectedTarget == null)
                {
                    var defaultTarget = SavedTarget.FindDefaultTarget();

                    if (defaultTarget != null)
                    {
                        _SelectedTarget = new Target(defaultTarget);
                    }
                }

                return _SelectedTarget;
            }
            set 
            { 
                _SelectedTarget = value;
                Events.FireSelectedTargetChanged(_SelectedTarget.Name);
            }
        }

        /// <summary>
        /// Gets the specified target by name.
        /// </summary>
        /// <param name="TargetName">The specified target name.</param>
        /// <returns>Returns true if target is found.</returns>
        public static Target? GetTarget(string TargetName)
        {
            var saveedTarget = SavedTarget.FindTarget(x => x.Name == TargetName);

            if (saveedTarget == null)
            {
                return null;
            }

            return new Target(saveedTarget);
        }

        /// <summary>
        /// Gets the specified target by predicate.
        /// </summary>
        /// <param name="predicate">The predicate to be used to find the target.</param>
        /// <returns></returns>
        public static Target? GetTarget(Expression<Func<SavedTarget, bool>> predicate)
        {
            var saveedTarget = SavedTarget.FindTarget(predicate);

            if (saveedTarget == null)
            {
                return null;
            }

            return new Target(saveedTarget);
        }

        /// <summary>
        /// Deletes the specified target.
        /// </summary>
        /// <param name="TargetName">The specified target name.</param>
        /// <returns>Returns true if the operation was successful.</returns>
        public static bool DeleteTarget(string TargetName)
        {
            var Target = SavedTarget.FindTarget(x => x.Name == TargetName);
            return Target.Remove();
        }

        /// <summary>
        /// Adds a new target.
        /// </summary>
        /// <param name="Default">If this target the new default.</param>
        /// <param name="TargetName">The mame for this target.</param>
        /// <param name="IPAddress">IP Address of this target.</param>
        /// <param name="PayloadPort">The payload port to be used for this target.</param>
        /// <returns>Returns true if successful.</returns>
        public static bool NewTarget(bool Default, string TargetName, string IPAddress, int PayloadPort)
        {
            return new SavedTarget { IsDefault = Default, Name = TargetName, IPAddress = IPAddress, PayloadPort = PayloadPort }.Add();
        }
    }
}
