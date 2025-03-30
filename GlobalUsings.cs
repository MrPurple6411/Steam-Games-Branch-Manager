global using System;
global using System.Collections.Generic;
global using System.ComponentModel;
global using System.IO;
global using System.Linq;
global using System.Windows.Forms;
global using System.Text.RegularExpressions;
global using System.Reflection;

// Add nullable reference attribute context for backward compatibility
namespace System.Runtime.CompilerServices
{
    // For .NET 9 compatibility
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method | AttributeTargets.Interface | AttributeTargets.Delegate, AllowMultiple = false, Inherited = false)]
    internal sealed class NullableContextAttribute : Attribute
    {
        public NullableContextAttribute(byte value) => Value = value;
        public byte Value { get; }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Event | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    internal sealed class NullableAttribute : Attribute
    {
        public NullableAttribute(byte A_1) => Value = new byte[] { A_1 };
        public NullableAttribute(byte[] A_1) => Value = A_1;
        public byte[] Value { get; }
    }

    // Additional required attributes
    [AttributeUsage(AttributeTargets.Module, AllowMultiple = false, Inherited = false)]
    internal sealed class NullablePublicOnlyAttribute : Attribute
    {
        public NullablePublicOnlyAttribute(bool isPublicOnly) => IsPublicOnly = isPublicOnly;
        public bool IsPublicOnly { get; }
    }
}
