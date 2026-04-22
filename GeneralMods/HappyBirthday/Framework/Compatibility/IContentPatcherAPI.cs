using System;
using System.Collections.Generic;
using StardewModdingAPI;
using StardewModdingAPI.Events;

namespace ContentPatcher;

/// <summary>The Content Patcher API which other mods can access.</summary>
public interface IContentPatcherAPI
{

    /// <summary>Register a simple token.</summary>
    /// <param name="mod">The manifest of the mod defining the token (see <see cref="Mod.ModManifest"/> in your entry class).</param>
    /// <param name="name">The token name. This only needs to be unique for your mod; Content Patcher will prefix it with your mod ID automatically, like <c>YourName.ExampleMod/SomeTokenName</c>.</param>
    /// <param name="getValue">A function which returns the current token value. If this returns a null or empty list, the token is considered unavailable in the current context and any patches or dynamic tokens using it are disabled.</param>
    void RegisterToken(IManifest mod, string name, Func<IEnumerable<string>?> getValue);

    /// <summary>Get a managed string which may contain Content Patcher tokens matched against Content Patcher's internal context.</summary>
    /// <param name="manifest">The manifest of the mod parsing the token string (see <see cref="Mod.ModManifest"/> in your entry class).</param>
    /// <param name="rawValue">The token string to parse, in the same format as strings in Content Patcher content packs.</param>
    /// <param name="formatVersion">The format version for which to parse the token string, used to ensure forward compatibility with future Content Patcher versions. See <c>Format</c> in the Content Patcher token documentation.</param>
    /// <param name="assumeModIds">
    /// <para>The unique IDs of mods whose custom tokens to allow in the <paramref name="rawValue"/>. You don't need to list the mod identified by <paramref name="manifest"/> or mods listed as a required dependency in the <paramref name="manifest"/>.</para>
    /// <para>NOTE: this is meant to prevent mods from breaking if a player doesn't have a required mod installed. You shouldn't simply list all installed mods, and parsing conditions will still fail if a mod isn't installed regardless of the listed mod IDs.</para>
    /// </param>
    IManagedTokenString ParseTokenString(IManifest manifest, string rawValue, ISemanticVersion formatVersion, string[]? assumeModIds = null);

}
