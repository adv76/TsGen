﻿using TsGen.Interfaces;
using TsGen.Models;

namespace TsGen.Builders.TypeBuilders
{
    public class InterfaceBuilder : ITypeBuilder
    {
        public TypeDef Build(Type type, bool export, TsGenSettings settings)
        {
            return new TypeDef(type);
        }
    }
}
