using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ContentPatcher;

/// <summary>A parsed string which may contain Content Patcher tokens matched against Content Patcher's internal context for an API consumer. This value is <strong>per-screen</strong>, so the result depends on the screen that's active when calling the members.</summary>
public interface IManagedTokenString
{
    /*********
    ** Accessors
    *********/
    /// <summary>Whether the token string was parsed successfully (regardless of whether its tokens are in scope currently).</summary>
    [MemberNotNullWhen(false, nameof(IManagedTokenString.ValidationError))]
    bool IsValid { get; }

    /// <summary>If <see cref="IsValid"/> is false, an error phrase indicating why the token string failed to parse, formatted like this: <c>'seasonz' isn't a valid token name; must be one of &lt;token list&gt;</c>. If the token string is valid, this is <c>null</c>.</summary>
    string? ValidationError { get; }

    /// <summary>Whether the token string's tokens are all valid in the current context. For example, this would be false if the token string use <c>{{Season}}</c> and a save isn't loaded yet.</summary>
    bool IsReady { get; }

    /// <summary>The parsed value for the current context.</summary>
    string? Value { get; }


    /*********
    ** Methods
    *********/
    /// <summary>Update the token string based on Content Patcher's current context for every active screen. It's safe to call this as often as you want, but it has no effect if the Content Patcher context hasn't changed since you last called it.</summary>
    /// <returns>Returns the screens for which <see cref="Value"/> changed.</returns>
    IEnumerable<int> UpdateContext();
}
