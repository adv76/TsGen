using TsGen.Enums;

namespace TsGen.Attributes
{
    /// <summary>
    /// This attribute is for customizing the property definition
    /// </summary>
    /// <remarks>
    /// This attribute can force an otherwise-excluded property to be included in the
    /// type definition. It can also be used to customize the type that is output or the
    /// optionality.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property)]
    public class TsPropGenAttribute : Attribute
    {
        private readonly string? _customType = null;
        private readonly Optionality? _optionality = null;

        /// <summary>
        /// Whether or not a custom type was specified.
        /// </summary>
        public bool HasCustomType => _customType is not null;

        /// <summary>
        /// The custom type that was specified.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Throws if a custom type was not specified. Use <see cref="HasCustomType" /> to check.
        /// </exception>
        public string CustomType => _customType is not null ? _customType : throw new InvalidOperationException();

        /// <summary>
        /// Whether or not a custom optionality was specified.
        /// </summary>
        public bool HasCustomOptionality => _optionality.HasValue;

        /// <summary>
        /// The custom optionality that was specified.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Throws if a custom optionality was not specified. Use <see cref="HasCustomOptionality" /> to check.
        /// </exception>
        public Optionality Nullability => _optionality.HasValue ? _optionality.Value : throw new InvalidOperationException();

        /// <summary>
        /// The default constructor for the attribute
        /// </summary>
        public TsPropGenAttribute() { }

        /// <summary>
        /// Alternate constructor that allows customizing the property definition.
        /// </summary>
        /// <param name="type">The custom type to use (or null for unspecified).</param>
        /// <param name="optionality">The custom optionality to use (or null for unspecified).</param>
        public TsPropGenAttribute(string? type = null, Optionality? optionality = null)
        {
            _customType = type;
            _optionality = optionality;
        }
    }
}
