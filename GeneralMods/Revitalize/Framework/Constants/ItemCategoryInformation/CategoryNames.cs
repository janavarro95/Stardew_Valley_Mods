using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omegasis.Revitalize.Framework.Constants.PathConstants;
using Omegasis.Revitalize.Framework.Utilities;
using Omegasis.Revitalize.Framework.Utilities.JsonContentLoading;
using StardewValley;

namespace Omegasis.Revitalize.Framework.Constants.ItemCategoryInformation
{
    /// <summary>
    /// A list of category names for objects.
    /// </summary>
    public static class CategoryNames
    {
        /// <summary>
        /// The category name for crafting station.
        /// </summary>
        public static string Crafting
        {
            get
            {
                return GetCategoryName("Crafting", "Crafting");
            }
        }

        public static string Farming
        {
            get
            {
                return GetCategoryName("Farming", "Farming");
            }
        }

        /// <summary>
        /// The category name for machines.
        /// </summary>
        public static string Machine
        {
            get
            {
                return GetCategoryName("Machine", "Machine");
            }
        }

        /// <summary>
        /// The category for miscelaneous objects.
        /// </summary>
        public static string Misc
        {
            get
            {
                return GetCategoryName("Misc", "Misc");
            }
        }

        /// <summary>
        /// The category name for Ore.
        /// </summary>
        public static string Ore
        {
            get
            {
                return GetCategoryName("Ore", "Ore");
            }
        }

        /// <summary>
        /// The category name for resources.
        /// </summary>
        public static string Resource
        {
            get
            {
                return GetCategoryName("Resource", "Resource");
            }
        }

        /// <summary>
        /// The category name for resources.
        /// </summary>
        public static string Storage
        {
            get
            {
                return GetCategoryName("Storage", "Storage");
            }
        }

        /// <summary>
        /// Gets a category name from a .json file.
        /// </summary>
        /// <param name="CategoryId">The id for the category to load the actual translated string from the file.</param>
        /// <param name="DefaultName">The default name to use if none is found.</param>
        /// <returns></returns>
        public static string GetCategoryName(string CategoryId, string DefaultName)
        {
            string catrgoryName = JsonContentPackUtilities.LoadCategory(CategoryId);
            if (string.IsNullOrEmpty(catrgoryName))
            {
                return DefaultName;
            }
            return catrgoryName;
        }
    }
}
