using TsGen.Enums;

namespace TsGen.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class TsPropGenAttribute : Attribute
    {
        private readonly string? _customType = null;

        public bool HasCustomType => _customType is not null;
        public string CustomType => _customType is not null ? _customType : throw new InvalidOperationException();
        public Optionality Nullability { get; private init; } = Optionality.Default;

        public TsPropGenAttribute() { }

        public TsPropGenAttribute(string? type = null, Optionality nullability = Optionality.Default)
        {
            _customType = type;
            Nullability = nullability;
        }
    }
}
