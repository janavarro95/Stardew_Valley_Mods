using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omegasis.HappyBirthday.Framework.Compatibility.ContentPatcher.Tokens
{
    /// <summary>
    /// Coppied/Implemented from https://github.com/Pathoschild/StardewMods/blob/develop/ContentPatcher/Framework/Tokens/ValueProviders/ModConvention/ConventionDelegates.cs.
    /// </summary>
    public class ContentPatcherTokenBase
    {

        /****
        ** Metadata
        ****/
        /// <summary>Get whether the values may change depending on the context.</summary>
        /// <remarks>Default true.</remarks>
        public virtual bool IsMutable()
        {
            return true;
        }

        /// <inheritdoc cref="IValueProvider.IsDeterministicForInput" />
        /// <remarks>Default false.</remarks>
        public virtual bool IsDeterministicForInput()
        {
            return false;
        }

        /// <summary>Get whether the token allows input arguments (e.g. an NPC name for a relationship token).</summary>
        /// <remarks>Default false.</remarks>
        public virtual bool AllowsInput()
        {
            return false;
        }

        /// <summary>Whether the token requires input arguments to work, and does not provide values without it (see <see cref="AllowsInput"/>).</summary>
        /// <remarks>Default false.</remarks>
        public virtual bool RequiresInput()
        {
            return false;
        }

        /// <summary>Whether the token may return multiple values for the given input.</summary>
        /// <param name="input">The input arguments, if any.</param>
        /// <remarks>Default true.</remarks>
        public virtual bool CanHaveMultipleValues(string? input = null)
        {
            return true;
        }

        /// <summary>Get the set of valid input arguments if restricted, or an empty collection if unrestricted.</summary>
        /// <remarks>Default unrestricted.</remarks>
        public virtual IEnumerable<string> GetValidInputs()
        {
            return Enumerable.Empty<string>();
        }

        /// <summary>Get whether the token always chooses from a set of known values for the given input. Mutually exclusive with <see cref="HasBoundedRangeValues"/>.</summary>
        /// <param name="input">The input arguments, if any.</param>
        /// <param name="allowedValues">The possible values for the input.</param>
        /// <remarks>Default unrestricted.</remarks>
        public virtual bool HasBoundedValues(string? input, out IEnumerable<string> allowedValues)
        {
            allowedValues = Enumerable.Empty<string>();
            return false;
        }

        /// <summary>Get whether the token always returns a value within a bounded numeric range for the given input. Mutually exclusive with <see cref="HasBoundedValues"/>.</summary>
        /// <param name="input">The input arguments, if any.</param>
        /// <param name="min">The minimum value this token may return.</param>
        /// <param name="max">The maximum value this token may return.</param>
        /// <remarks>Default false.</remarks>
        public virtual bool HasBoundedRangeValues(string? input, out int min, out int max)
        {
            min = 0;
            max = 0;
            return false;
        }

        /// <summary>Validate that the provided input arguments are valid.</summary>
        /// <param name="input">The input arguments, if any.</param>
        /// <param name="error">The validation error, if any.</param>
        /// <returns>Returns whether validation succeeded.</returns>
        /// <remarks>Default true.</remarks>
        public virtual bool TryValidateInput(string? input, [NotNullWhen(false)] out string? error)
        {
            error = "";
            return true;
        }

        /// <summary>Validate that the provided values are valid for the given input arguments (regardless of whether they match).</summary>
        /// <param name="input">The input arguments, if any.</param>
        /// <param name="values">The values to validate.</param>
        /// <param name="error">The validation error, if any.</param>
        /// <returns>Returns whether validation succeeded.</returns>
        /// <remarks>Default true.</remarks>
        public virtual bool TryValidateValues(string? input, IEnumerable<string> values, [NotNullWhen(false)] out string? error)
        {
            error = "";
            return true;
        }
        /*
        /// <summary>Normalize a token value so it matches the format expected by the value provider, if needed.</summary>
        /// <param name="value">This receives the raw value, already trimmed and non-empty.</param>
        public virtual string NormalizeValue(string value)
        {
            return value;
        }
        */


        /****
        ** State
        ****/
        /// <summary>Update the values when the context changes.</summary>
        /// <returns>Returns whether the value changed, which may trigger patch updates.</returns>
        public virtual bool UpdateContext()
        {
            return false;
        }

        /// <summary>Get whether the token is available for use.</summary>
        public virtual bool IsReady()
        {
            return true;
        }

        /// <summary>Get the current values.</summary>
        /// <param name="input">The input arguments, if any.</param>
        public virtual IEnumerable<string> GetValues(string? input)
        {
            return Enumerable.Empty<string>();
        }
    }
}
