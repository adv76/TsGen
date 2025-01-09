using TsGen.Enums;

namespace TsGen.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class TsPropGenAttribute : Attribute
    {
        private readonly string? _customType = null;

        public bool HasCustomType => _customType is not null;
        public string CustomType => _customType is not null ? _customType : throw new InvalidOperationException();
        public Nullability Nullability { get; private init; } = Nullability.Default;

        public TsPropGenAttribute() { }

        public TsPropGenAttribute(string? type = null, Nullability nullability = Nullability.Default)
        {
            _customType = type;
            Nullability = nullability;
        }
    }
}
