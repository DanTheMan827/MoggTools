using System;
using System.Runtime.InteropServices;

namespace LibMoggCrypt
{
	[StructLayout(LayoutKind.Explicit)]
	internal struct BytetoIntConverter
	{
		[FieldOffset(0)]
		public byte[] asBytes;

		[FieldOffset(0)]
		public int[] asInts;
	}
}