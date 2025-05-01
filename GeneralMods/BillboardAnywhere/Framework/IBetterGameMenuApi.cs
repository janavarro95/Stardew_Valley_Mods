#nullable enable

using StardewValley.Menus;

namespace Leclair.Stardew.BetterGameMenu;


public interface IBetterGameMenuApi
{

    #region Menu Class Access

    /// <summary>
    /// The current page of the active screen's current Better Game Menu,
    /// if one is open, else <c>null</c>. This exists as a quicker alternative
    /// to <c>ActiveMenu?.CurrentPage</c> with the additional benefit that
    /// you can prune <c>IBetterGameMenu</c> from your copy of the API
    /// file if you're not using anything else from it.
    /// </summary>
    IClickableMenu? ActivePage { get; }

    #endregion

}
