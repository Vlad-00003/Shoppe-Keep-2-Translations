﻿using System.Collections.Generic;
using System.Linq;

namespace Share
{
    internal static class Comparator
    {
        private static bool Equal(this Dictionary<string, string> target, Dictionary<string, string> orig)
        {
            return target != null && orig?.All(p => target.ContainsKey(p.Key)) == true;
        }

        /// <summary>
        /// Fills the target with missing keys from orig,
        /// Returns if the file was modified.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="orig"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public static bool FillKeys(this Dictionary<string, string> target, Dictionary<string, string> orig,ILogger logger)
        {
            if (Equal(target, orig))
            {
                return true;
            }
            var diff = orig.Keys.Except(target.Keys).ToList();
            foreach (var key in diff)
            {
                logger.PrintWarning("Adding key \"{0}\" with English value.",key);
                target.Add(key,orig[key]);
            }
            return false;
        }

        public static void SortDict(this Dictionary<string, string> target, Dictionary<string, string> orig, ILogger logger)
        {
            var bckp = new Dictionary<string,string>(target);
            target.Clear();
            foreach (var pair in orig)
            {
                target.Add(pair.Key,bckp[pair.Key]);
                bckp.Remove(pair.Key);
            }
            foreach (var pair in bckp)
            {
                if (target.ContainsKey(pair.Key))
                {
                    logger.PrintWarning("Duplicate key \"{0}\" while sorting the dictionary! This should never happen!",pair.Key);
                    continue;
                }
                target.Add(pair.Key,pair.Value);
            }
        }
    }
}
