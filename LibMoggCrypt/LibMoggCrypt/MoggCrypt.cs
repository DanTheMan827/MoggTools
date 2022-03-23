using LibOrbisPkg.Util;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace LibMoggCrypt
{
	public class MoggCrypt
	{
		private static byte[] ctr_key_0B;

		private static byte[] HvKey0C;

		private static byte[] HvKey0E;

		private static byte[] HvKey0F;

		private static byte[] HvKey10;

		private static byte[] HvKey11;

		private static byte[] hiddenKeys;

		private static byte[] hiddenKeys_17;

		private static byte[] HvKey_transform;

		static MoggCrypt()
		{
			MoggCrypt.ctr_key_0B = new byte[] { 55, 178, 226, 185, 28, 116, 250, 158, 56, 129, 8, 234, 54, 35, 219, 228 };
			MoggCrypt.HvKey0C = new byte[] { 1, 34, 0, 56, 210, 1, 120, 139, 221, 205, 208, 240, 254, 62, 36, 127 };
			MoggCrypt.HvKey0E = new byte[] { 81, 115, 173, 229, 179, 153, 184, 97, 88, 26, 249, 184, 30, 167, 190, 191 };
			MoggCrypt.HvKey0F = new byte[] { 198, 34, 148, 48, 216, 60, 132, 20, 8, 115, 124, 242, 35, 246, 235, 90 };
			MoggCrypt.HvKey10 = new byte[] { 2, 26, 131, 243, 151, 233, 212, 184, 6, 116, 20, 107, 48, 76, 0, 145 };
			MoggCrypt.HvKey11 = new byte[] { 66, 102, 55, 179, 104, 5, 159, 133, 110, 150, 189, 30, 249, 14, 127, 189 };
			MoggCrypt.hiddenKeys = new byte[] { 127, 149, 91, 157, 148, 186, 18, 241, 215, 90, 103, 217, 22, 69, 40, 221, 97, 85, 85, 175, 35, 145, 214, 10, 58, 66, 129, 24, 180, 247, 243, 4, 120, 150, 93, 146, 146, 176, 71, 172, 143, 91, 109, 220, 28, 65, 126, 218, 106, 85, 83, 175, 32, 200, 220, 10, 102, 67, 221, 28, 178, 165, 164, 12, 126, 146, 92, 147, 144, 237, 74, 173, 139, 7, 54, 211, 16, 65, 120, 143, 96, 8, 85, 168, 38, 207, 208, 15, 101, 17, 132, 69, 177, 160, 250, 87, 121, 151, 11, 144, 146, 176, 68, 173, 138, 14, 96, 217, 20, 17, 126, 141, 53, 93, 92, 251, 33, 156, 211, 14, 50, 64, 209, 72, 184, 167, 161, 13, 40, 195, 93, 151, 193, 236, 66, 241, 220, 93, 55, 218, 20, 71, 121, 138, 50, 92, 84, 242, 114, 157, 211, 13, 103, 76, 214, 73, 180, 162, 243, 80, 40, 150, 94, 149, 197, 233, 69, 173, 138, 93, 100, 142, 23, 64, 46, 135, 54, 88, 6, 253, 117, 144, 208, 95, 58, 64, 212, 76, 176, 247, 167, 4, 44, 150, 1, 150, 155, 188, 21, 166, 222, 14, 101, 141, 23, 71, 47, 221, 99, 84, 85, 175, 118, 202, 132, 95, 98, 68, 128, 74, 179, 244, 244, 12, 126, 196, 14, 198, 154, 235, 67, 160, 219, 10, 100, 223, 28, 66, 36, 137, 99, 92, 85, 243, 113, 144, 220, 93, 96, 64, 209, 77, 178, 163, 167, 13, 44, 154, 11, 144, 154, 190, 71, 167, 136, 90, 109, 223, 19, 29, 46, 139, 96, 94, 85, 242, 116, 156, 215, 14, 96, 64, 128, 28, 183, 161, 244, 2, 40, 150, 91, 149, 193, 233, 64, 163, 143, 12, 50, 223, 67, 29, 36, 141, 97, 9, 84, 171, 39, 154, 211, 88, 96, 22, 132, 79, 179, 164, 243, 13, 37, 147, 8, 192, 154, 189, 16, 162, 214, 9, 96, 143, 17, 29, 122, 143, 99, 11, 93, 242, 33, 236, 215, 8, 98, 64, 132, 73, 176, 173, 242, 7, 41, 195, 12, 150, 150, 235, 16, 160, 218, 89, 50, 211, 23, 65, 37, 220, 99, 8, 4, 174, 119, 203, 132, 90, 96, 77, 221, 69, 181, 244, 160, 5 };
			MoggCrypt.hiddenKeys_17 = new byte[] { 83, 182, 46, 244, 231, 236, 70, 10, 210, 167, 154, 183, 111, 0, 182, 232, 4, 109, 40, 208, 243, 175, 166, 93, 229, 39, 185, 6, 182, 105, 162, 214, 27, 241, 51, 136, 198, 206, 153, 248, 114, 58, 57, 148, 220, 89, 116, 156, 65, 145, 101, 201, 85, 214, 76, 166, 82, 5, 215, 171, 233, 218, 61, 92, 218, 86, 27, 182, 43, 193, 34, 145, 6, 178, 166, 92, 188, 79, 80, 75, 61, 106, 17, 205, 202, 234, 171, 91, 105, 140, 191, 147, 211, 247, 85, 230, 115, 146, 201, 217, 227, 82, 93, 86, 116, 115, 248, 170, 207, 203, 239, 93, 233, 200, 151, 150, 220, 126, 199, 247, 212, 131, 155, 157, 144, 6, 181, 96, 119, 153, 169, 15, 131, 155, 26, 221, 188, 96, 83, 238, 244, 250, 119, 150, 208, 15, 143, 75, 187, 46, 131, 245, 25, 39, 194, 168, 16, 64, 240, 243, 170, 225, 157, 241, 96, 56, 249, 225, 52, 16, 167, 133, 227, 154, 119, 199, 17, 156, 235, 113, 113, 193, 43, 14, 149, 46, 12, 167, 148, 105, 11, 86, 134, 98, 242, 119, 208, 51, 144, 88, 248, 34, 227, 221, 72, 180, 152, 254, 158, 223, 71, 114, 168, 56, 21, 61, 139, 17, 227, 221, 255, 249, 84, 157, 163, 46, 230, 84, 52, 148, 143, 61, 108, 120, 192, 6, 40, 233, 132, 90, 128, 184, 190, 187, 3, 177, 27, 182, 220, 182, 76, 213, 226, 191, 120, 47, 53, 129, 134, 201, 66, 203, 27, 43, 135, 50, 174, 152, 115, 142, 206, 2, 167, 136, 44, 190, 250, 84, 157, 132, 190, 196, 11, 255, 230, 217, 24, 46, 202, 83, 176, 95, 20, 58, 64, 178, 95, 141, 94, 16, 134, 13, 99, 189, 199, 75, 113, 214, 255, 221, 45, 31, 217, 6, 32, 246, 248, 47, 125, 86, 64, 47, 147, 102, 155, 238, 41, 92, 145, 207, 166, 173, 71, 99, 1, 135, 81, 108, 232, 41, 85, 104, 94, 17, 194, 72, 35, 150, 5, 120, 179, 161, 143, 251, 126, 173, 105, 106, 36, 205, 3, 151, 202, 184, 72, 57, 246, 221, 86, 128, 97, 231, 102, 238, 92, 85, 209, 82, 87, 206, 210, 192, 190, 193 };
			MoggCrypt.HvKey_transform = new byte[] { 57, 162, 191, 83, 125, 136, 29, 3, 53, 56, 163, 128, 69, 36, 238, 202, 37, 109, 165, 194, 101, 169, 148, 115, 229, 116, 235, 84, 229, 149, 63, 28 };
		}

		public MoggCrypt()
		{
		}

		private static byte asciiDigitToHexInv(byte a)
		{
			if (a - 97 > 5)
			{
			}
			return (byte)(a - 87);
		}

		internal static void doCrypt(byte[] key, byte[] moggData, byte[] file_nonce, int oggOffset)
		{
			byte[] numArray = new byte[16];
			int num = 0;
			int num1 = 0;
			int j = 0;
			using (AesManaged aesManaged = new AesManaged())
			{
				aesManaged.Mode = CipherMode.ECB;
				aesManaged.BlockSize = 128;
				aesManaged.KeySize = 128;
				aesManaged.Padding = PaddingMode.None;
				aesManaged.Key = key;
				ICryptoTransform cryptoTransform = aesManaged.CreateEncryptor();
				cryptoTransform.TransformBlock(file_nonce, 0, (int)file_nonce.Length, numArray, 0);
				for (int i = oggOffset; i < (int)moggData.Length; i++)
				{
					if (num == 16)
					{
						num1 = 0;
						for (j = 0; j < 16; j++)
						{
							ref byte fileNonce = ref file_nonce[num1];
							fileNonce = (byte)(fileNonce + 1);
							if (file_nonce[num1] != 0)
							{
								break;
							}
							num1++;
						}
						cryptoTransform.TransformBlock(file_nonce, 0, (int)file_nonce.Length, numArray, 0);
						num = 0;
					}
					moggData[i] = (byte)(moggData[i] ^ numArray[num]);
					num++;
				}
			}
		}

		public static uint flip_endianness(uint x)
		{
			x = x >> 16 | x << 16;
			return (uint)((x & -16711936) >> 8 | (x & 16711935) << 8);
		}

		internal static byte[] gen_key(byte[] HvKey, byte[] moggData, int version)
		{
			byte[] numArray;
			using (MemoryStream memoryStream = new MemoryStream(moggData))
			{
				numArray = MoggCrypt.gen_key(HvKey, memoryStream, version);
			}
			return numArray;
		}

		internal static byte[] gen_key(byte[] HvKey, Stream mogg, int version)
		{
			byte[] numArray = new byte[16];
			byte[] numArray1 = new byte[32];
			byte[] numArray2 = new byte[16];
			mogg.Position = (long)16;
			int num = mogg.ReadInt32LE();
			mogg.Position = (long)(20 + num * 8 + 16 + 32);
			mogg.Read(numArray, 0, 16);
			using (AesManaged aesManaged = new AesManaged())
			{
				aesManaged.Mode = CipherMode.ECB;
				aesManaged.BlockSize = 128;
				aesManaged.KeySize = 128;
				aesManaged.Padding = PaddingMode.None;
				aesManaged.Key = HvKey;
				aesManaged.CreateDecryptor().TransformBlock(numArray, 0, (int)numArray.Length, numArray, 0);
			}
			mogg.Position = (long)(20 + num * 8 + 16);
			int num1 = mogg.ReadInt32LE();
			mogg.Position = (long)(20 + num * 8 + 16 + 8);
			int num2 = mogg.ReadInt32LE();
			mogg.Position = (long)(20 + num * 8 + 16 + 48);
			if (version == 17)
			{
				ulong num3 = mogg.ReadUInt64LE();
				if (num3 != (long)4 && num3 != (long)1)
				{
					throw new Exception(string.Format("v17 mogg had {0} at unknown value, previously only seen 1 and 4", num3));
				}
			}
			int num4 = mogg.ReadInt32LE();
			num4 = num4 % 6 + 6;
			Array.Copy((version >= 17 ? MoggCrypt.hiddenKeys_17 : MoggCrypt.hiddenKeys), 32 * num4, numArray1, 0, 32);
			numArray1 = MoggCrypt.revealKey(MoggCrypt.HvKey_transform, numArray1);
			Func<byte, byte> func = (version >= 17 ? new Func<byte, byte>(MoggCrypt.asciiDigitToHexInv) : new Func<byte, byte>(MoggCrypt.hex_char_to_nibble));
			numArray2 = MoggCrypt.grind_array(num1, num2, MoggCrypt.hex_string_to_bytes(numArray1, func), version);
			byte[] numArray3 = new byte[16];
			for (int i = 0; i < 16; i++)
			{
				numArray3[i] = (byte)(numArray2[i] ^ numArray[i]);
			}
			return numArray3;
		}

		public static Stream GetOggCryptStream(Stream mogg)
		{
			mogg.Position = (long)0;
			uint num = mogg.ReadUInt32LE();
			if (num != 10)
			{
				if (num > 16)
				{
					throw new InvalidDataException("Not a valid mogg");
				}
				return new MoggCrypt.VorbisCryptStream(mogg);
			}
			int num1 = mogg.ReadInt32LE();
			return new OffsetStream(mogg, (long)num1, mogg.Length - (long)num1);
		}

		private static byte[] grind_array(int a1, int a2, byte[] bar, int version)
		{
			int i;
			int num;
			byte[] numArray = new byte[64];
			byte[] numArray1 = new byte[64];
			int num1 = a1;
			int num2 = a2;
			int[] numArray2 = new int[256];
			for (i = 0; i < 256; i++)
			{
				numArray2[i] = (byte)((byte)a1 >> 3);
				a1 = 1664525 * a1 + 1013904223;
			}
			if (a2 == 0)
			{
				a2 = 12351;
			}
			for (i = 0; i < 32; i++)
			{
				do
				{
					a2 = 1664525 * a2 + 1013904223;
					num = a2 >> 2 & 31;
				}
				while (numArray[num] != 0);
				numArray1[i] = (byte)num;
				numArray[num] = 1;
			}
			int[] numArray3 = numArray2;
			int[] numArray4 = new int[256];
			a1 = num2;
			for (i = 0; i < 256; i++)
			{
				numArray4[i] = (byte)((byte)a1 >> 2 & 63);
				a1 = 1664525 * a1 + 1013904223;
			}
			if (version > 13)
			{
				for (i = 32; i < 64; i++)
				{
					do
					{
						num1 = 1664525 * num1 + 1013904223;
						num = (num1 >> 2 & 31) + 32;
					}
					while (numArray[num] != 0);
					numArray1[i] = (byte)num;
					numArray[num] = 1;
				}
				numArray3 = numArray4;
			}
			for (int j = 0; j < 16; j++)
			{
				byte num3 = bar[j];
				for (int k = 0; k < 16; k += 2)
				{
					num3 = MoggCrypt.O_funcs(num3, bar[k + 1], (int)numArray1[numArray3[bar[k]]]);
				}
				bar[j] = num3;
			}
			return bar;
		}

		private static byte hex_char_to_nibble(byte h)
		{
			if (h > 96)
			{
				return (byte)(h - 87);
			}
			if (h > 64)
			{
				return (byte)(h - 55);
			}
			if (h <= 47)
			{
				return (byte)0;
			}
			return (byte)(h - 48);
		}

		private static byte[] hex_string_to_bytes(byte[] h, Func<byte, byte> transformer = null)
		{
			byte[] numArray = new byte[16];
			transformer = transformer ?? new Func<byte, byte>(MoggCrypt.hex_char_to_nibble);
			for (int i = 0; i < 16; i++)
			{
				numArray[i] = (byte)(transformer(h[i * 2]) << 4 | transformer(h[i * 2 + 1]));
			}
			return numArray;
		}

		internal static void HMXA2OGG(ref byte[] moggData, int moggStart, int numEntries)
		{
			int num = BitConverter.ToInt32(moggData, 20 + numEntries * 8 + 16);
			int num1 = BitConverter.ToInt32(moggData, 20 + numEntries * 8 + 16 + 8);
			int num2 = 1664525 * (1664525 * (num ^ 1549556828) + 1013904223) + 1013904223;
			int num3 = 1664525 * (num1 ^ 909522486) + 1013904223;
			num2 = (int)MoggCrypt.flip_endianness((uint)num2);
			num3 = (int)MoggCrypt.flip_endianness((uint)num3);
			BytetoIntConverter bytetoIntConverter = new BytetoIntConverter()
			{
				asBytes = moggData
			};
			bytetoIntConverter.asInts[moggStart / 4] = 1399285583;
			bytetoIntConverter.asInts[moggStart / 4 + 3] ^= num2;
			bytetoIntConverter.asInts[moggStart / 4 + 5] ^= num3;
		}

		public static MoggCryptResult nativeDecrypt(byte[] moggData)
		{
			byte[] ctrKey0B = new byte[16];
			switch (moggData[0])
			{
				case 10:
				{
					return MoggCryptResult.ERR_ALREADY_DECRYPTED;
				}
				case 11:
				{
					ctrKey0B = MoggCrypt.ctr_key_0B;
					break;
				}
				case 12:
				case 13:
				{
					ctrKey0B = MoggCrypt.gen_key(MoggCrypt.HvKey0C, moggData, 12);
					break;
				}
				case 14:
				{
					ctrKey0B = MoggCrypt.gen_key(MoggCrypt.HvKey0E, moggData, 14);
					break;
				}
				case 15:
				{
					ctrKey0B = MoggCrypt.gen_key(MoggCrypt.HvKey0F, moggData, 15);
					break;
				}
				case 16:
				{
					ctrKey0B = MoggCrypt.gen_key(MoggCrypt.HvKey10, moggData, 16);
					break;
				}
				case 17:
				{
					ctrKey0B = MoggCrypt.gen_key(MoggCrypt.HvKey11, moggData, 17);
					break;
				}
				default:
				{
					return MoggCryptResult.ERR_UNSUPPORTED_ENCRYPTION;
				}
			}
			int num = BitConverter.ToInt32(moggData, 4);
			int num1 = BitConverter.ToInt32(moggData, 16);
			byte[] numArray = new byte[16];
			Array.Copy(moggData, 20 + num1 * 8, numArray, 0, 16);
			MoggCrypt.doCrypt(ctrKey0B, moggData, numArray, num);

			if (moggData[num] == 72)
			{
				MoggCrypt.HMXA2OGG(ref moggData, num, num1);
			}
			else if (moggData[num] != 79)
			{
				return MoggCryptResult.ERR_DECRYPT_FAILED;
			}
			moggData[0] = 10;
			return MoggCryptResult.SUCCESS;
		}

		private static byte O_funcs(byte a1, byte a2, int op)
		{
			byte num;
			object obj;
			int num1 = 0;
			switch (op)
			{
				case 0:
				{
					num1 = a2 + MoggCrypt.rotr(a1, (a2 == 0 ? 1 : 0));
					break;
				}
				case 1:
				{
					num1 = a2 + MoggCrypt.rotr(a1, 3);
					break;
				}
				case 2:
				{
					num1 = a2 + MoggCrypt.rotl(a1, 1);
					break;
				}
				case 3:
				{
					num1 = a2 ^ (a1 >> (a2 & 7 & 31) | (byte)(a1 << (-a2 & 7 & 31)));
					break;
				}
				case 4:
				{
					num1 = a2 ^ MoggCrypt.rotl(a1, 4);
					break;
				}
				case 5:
				{
					num = MoggCrypt.rotr(a1, 3);
					num1 = a2 + (a2 ^ num);
					break;
				}
				case 6:
				{
					num1 = a2 + MoggCrypt.rotl(a1, 2);
					break;
				}
				case 7:
				{
					num1 = a2 + (a1 == 0 ? 1 : 0);
					break;
				}
				case 8:
				{
					num1 = a2 ^ MoggCrypt.rotr(a1, (a2 == 0 ? 1 : 0));
					break;
				}
				case 9:
				{
					num = MoggCrypt.rotl(a1, 3);
					num1 = a2 ^ a2 + num;
					break;
				}
				case 10:
				{
					num1 = a2 + MoggCrypt.rotl(a1, 3);
					break;
				}
				case 11:
				{
					num1 = a2 + MoggCrypt.rotl(a1, 4);
					break;
				}
				case 12:
				{
					num1 = a1 ^ a2;
					break;
				}
				case 13:
				{
					num1 = a2 ^ (a1 == 0 ? 1 : 0);
					break;
				}
				case 14:
				{
					num = MoggCrypt.rotr(a1, 3);
					num1 = a2 ^ a2 + num;
					break;
				}
				case 15:
				{
					num1 = a2 ^ MoggCrypt.rotl(a1, 3);
					break;
				}
				case 16:
				{
					num1 = a2 ^ MoggCrypt.rotl(a1, 2);
					break;
				}
				case 17:
				{
					num = MoggCrypt.rotl(a1, 3);
					num1 = a2 + (a2 ^ num);
					break;
				}
				case 18:
				{
					num1 = a2 + (a1 ^ a2);
					break;
				}
				case 19:
				{
					num1 = a1 + a2;
					break;
				}
				case 20:
				{
					num1 = a2 ^ MoggCrypt.rotr(a1, 3);
					break;
				}
				case 21:
				{
					num1 = a2 ^ a1 + a2;
					break;
				}
				case 22:
				{
					num1 = MoggCrypt.rotr(a1, (a2 == 0 ? 1 : 0));
					break;
				}
				case 23:
				{
					num1 = a2 + MoggCrypt.rotr(a1, 1);
					break;
				}
				case 24:
				{
					num1 = a1 >> (a2 & 7 & 31) | a1 << (-a2 & 7 & 31);
					break;
				}
				case 25:
				{
					if (a1 == 0)
					{
						obj = (a2 == 0 ? 128 : 1);
					}
					else
					{
						obj = null;
					}
					num1 = (byte)obj;
					break;
				}
				case 26:
				{
					num1 = a2 + MoggCrypt.rotr(a1, 2);
					break;
				}
				case 27:
				{
					num1 = a2 ^ MoggCrypt.rotr(a1, 1);
					break;
				}
				case 28:
				{
					a1 = (byte)(~a1);
					goto case 24;
				}
				case 29:
				{
					num1 = a2 ^ MoggCrypt.rotr(a1, 2);
					break;
				}
				case 30:
				{
					num1 = a2 + (a1 >> (a2 & 7 & 31) | (byte)(a1 << (-a2 & 7 & 31)));
					break;
				}
				case 31:
				{
					num1 = a2 ^ MoggCrypt.rotl(a1, 1);
					break;
				}
				case 32:
				{
					num1 = (byte)((a1 << 8 | 170 | a1 ^ 255) >> 4) ^ a2;
					break;
				}
				case 33:
				{
					num1 = (byte)((a1 ^ 255 | a1 << 8) >> 3 ^ a2);
					break;
				}
				case 34:
				{
					num1 = (byte)((a1 << 8 ^ 65280 | a1) >> 2 ^ a2);
					break;
				}
				case 35:
				{
					num1 = (byte)((a1 ^ 92 | a1 << 8) >> 5 ^ a2);
					break;
				}
				case 36:
				{
					num1 = (byte)((a1 << 8 | 101 | a1 ^ 60) >> 2 ^ a2);
					break;
				}
				case 37:
				{
					num1 = (byte)((a1 ^ 54 | a1 << 8) >> 2 ^ a2);
					break;
				}
				case 38:
				{
					num1 = (byte)((a1 ^ 54 | a1 << 8) >> 4 ^ a2);
					break;
				}
				case 39:
				{
					num1 = (byte)((a1 ^ 92 | a1 << 8 | 54) >> 1 ^ a2);
					break;
				}
				case 40:
				{
					num1 = (byte)((a1 ^ 255 | a1 << 8) >> 5 ^ a2);
					break;
				}
				case 41:
				{
					num1 = (byte)((~a1 << 8 | a1) >> 6 ^ a2);
					break;
				}
				case 42:
				{
					num1 = (byte)((a1 ^ 92 | a1 << 8) >> 3 ^ a2);
					break;
				}
				case 43:
				{
					num1 = (byte)((a1 ^ 60 | 101 | a1 << 8) >> 5 ^ a2);
					break;
				}
				case 44:
				{
					num1 = (byte)((a1 ^ 54 | a1 << 8) >> 1 ^ a2);
					break;
				}
				case 45:
				{
					num1 = (byte)((a1 ^ 101 | a1 << 8 | 60) >> 6 ^ a2);
					break;
				}
				case 46:
				{
					num1 = (byte)((a1 ^ 92 | a1 << 8) >> 2 ^ a2);
					break;
				}
				case 47:
				{
					num1 = (byte)((a2 ^ 170 | a2 << 8 | 255) >> 3 ^ a1);
					break;
				}
				case 48:
				{
					num1 = (byte)((a1 ^ 99 | a1 << 8 | 92) >> 6 ^ a2);
					break;
				}
				case 49:
				{
					num1 = (byte)((a1 ^ 92 | a1 << 8 | 54) >> 7 ^ a2);
					break;
				}
				case 50:
				{
					num1 = (byte)((a1 ^ 92 | a1 << 8) >> 6 ^ a2);
					break;
				}
				case 51:
				{
					num1 = (byte)((a1 << 8 ^ 65280 | a1) >> 3 ^ a2);
					break;
				}
				case 52:
				{
					num1 = (byte)((a1 ^ 255 | a1 << 8) >> 6 ^ a2);
					break;
				}
				case 53:
				{
					num1 = (byte)((a1 << 8 ^ 65280 | a1) >> 5 ^ a2);
					break;
				}
				case 54:
				{
					num1 = (byte)((a1 ^ 60 | 101 | a1 << 8) >> 4 ^ a2);
					break;
				}
				case 55:
				{
					num1 = (byte)((a1 ^ 99 | a1 << 8 | 92) >> 3 ^ a2);
					break;
				}
				case 56:
				{
					num1 = (byte)((a1 ^ 99 | a1 << 8 | 92) >> 5 ^ a2);
					break;
				}
				case 57:
				{
					num1 = (byte)((a1 ^ 175 | a1 << 8 | 250) >> 5 ^ a2);
					break;
				}
				case 58:
				{
					num1 = (byte)((a1 ^ 92 | a1 << 8 | 54) >> 5 ^ a2);
					break;
				}
				case 59:
				{
					num1 = (byte)((a1 ^ 92 | a1 << 8 | 54) >> 3 ^ a2);
					break;
				}
				case 60:
				{
					num1 = (byte)((a1 ^ 54 | a1 << 8) >> 3 ^ a2);
					break;
				}
				case 61:
				{
					num1 = (byte)((a1 ^ 99 | a1 << 8 | 92) >> 4 ^ a2);
					break;
				}
				case 62:
				{
					num1 = (byte)((a1 ^ 255 | a1 << 8 | 175) >> 6 ^ a2);
					break;
				}
				case 63:
				{
					num1 = (byte)((a1 ^ 255 | a1 << 8) >> 2 ^ a2);
					break;
				}
			}
			return (byte)(num1 & 255);
		}

		private static byte[] revealKey(byte[] transform, byte[] buf32)
		{
			for (int i = 0; i < 14; i++)
			{
				MoggCrypt.swap(ref buf32, 19, 2);
				MoggCrypt.swap(ref buf32, 22, 1);
				MoggCrypt.swap(ref buf32, 23, 6);
				MoggCrypt.swap(ref buf32, 26, 5);
				MoggCrypt.swap(ref buf32, 27, 10);
				MoggCrypt.swap(ref buf32, 30, 9);
				MoggCrypt.swap(ref buf32, 31, 14);
				MoggCrypt.swap(ref buf32, 2, 13);
				MoggCrypt.swap(ref buf32, 3, 18);
				MoggCrypt.swap(ref buf32, 6, 17);
				MoggCrypt.swap(ref buf32, 7, 22);
				MoggCrypt.swap(ref buf32, 10, 21);
				MoggCrypt.swap(ref buf32, 11, 26);
				MoggCrypt.swap(ref buf32, 14, 25);
				MoggCrypt.swap(ref buf32, 15, 30);
				MoggCrypt.swap(ref buf32, 18, 29);
				MoggCrypt.swap(ref buf32, 29, 2);
				MoggCrypt.swap(ref buf32, 28, 3);
				MoggCrypt.swap(ref buf32, 25, 6);
				MoggCrypt.swap(ref buf32, 24, 7);
				MoggCrypt.swap(ref buf32, 21, 10);
				MoggCrypt.swap(ref buf32, 20, 11);
				MoggCrypt.swap(ref buf32, 17, 14);
				MoggCrypt.swap(ref buf32, 16, 15);
				MoggCrypt.swap(ref buf32, 13, 18);
				MoggCrypt.swap(ref buf32, 12, 19);
				MoggCrypt.swap(ref buf32, 9, 22);
				MoggCrypt.swap(ref buf32, 8, 23);
				MoggCrypt.swap(ref buf32, 5, 26);
				MoggCrypt.swap(ref buf32, 4, 27);
				MoggCrypt.swap(ref buf32, 1, 30);
				MoggCrypt.swap(ref buf32, 0, 31);
				MoggCrypt.swap(ref buf32, 16, 2);
				MoggCrypt.swap(ref buf32, 28, 3);
				MoggCrypt.swap(ref buf32, 12, 6);
				MoggCrypt.swap(ref buf32, 24, 7);
				MoggCrypt.swap(ref buf32, 8, 10);
				MoggCrypt.swap(ref buf32, 20, 11);
				MoggCrypt.swap(ref buf32, 4, 14);
				MoggCrypt.swap(ref buf32, 16, 15);
				MoggCrypt.swap(ref buf32, 0, 18);
				MoggCrypt.swap(ref buf32, 12, 19);
				MoggCrypt.swap(ref buf32, 28, 22);
				MoggCrypt.swap(ref buf32, 8, 23);
				MoggCrypt.swap(ref buf32, 24, 26);
				MoggCrypt.swap(ref buf32, 4, 27);
				MoggCrypt.swap(ref buf32, 20, 30);
				MoggCrypt.swap(ref buf32, 0, 31);
				MoggCrypt.swap(ref buf32, 29, 2);
				MoggCrypt.swap(ref buf32, 15, 3);
				MoggCrypt.swap(ref buf32, 25, 6);
				MoggCrypt.swap(ref buf32, 11, 7);
				MoggCrypt.swap(ref buf32, 21, 10);
				MoggCrypt.swap(ref buf32, 7, 11);
				MoggCrypt.swap(ref buf32, 17, 14);
				MoggCrypt.swap(ref buf32, 3, 15);
				MoggCrypt.swap(ref buf32, 13, 18);
				MoggCrypt.swap(ref buf32, 31, 19);
				MoggCrypt.swap(ref buf32, 9, 22);
				MoggCrypt.swap(ref buf32, 27, 23);
				MoggCrypt.swap(ref buf32, 5, 26);
				MoggCrypt.swap(ref buf32, 23, 27);
				MoggCrypt.swap(ref buf32, 1, 30);
				MoggCrypt.swap(ref buf32, 19, 31);
				MoggCrypt.swap(ref buf32, 29, 21);
				MoggCrypt.swap(ref buf32, 28, 3);
				MoggCrypt.swap(ref buf32, 25, 25);
				MoggCrypt.swap(ref buf32, 24, 7);
				MoggCrypt.swap(ref buf32, 21, 29);
				MoggCrypt.swap(ref buf32, 20, 11);
				MoggCrypt.swap(ref buf32, 17, 1);
				MoggCrypt.swap(ref buf32, 16, 15);
				MoggCrypt.swap(ref buf32, 13, 5);
				MoggCrypt.swap(ref buf32, 12, 19);
				MoggCrypt.swap(ref buf32, 9, 9);
				MoggCrypt.swap(ref buf32, 8, 23);
				MoggCrypt.swap(ref buf32, 5, 13);
				MoggCrypt.swap(ref buf32, 4, 27);
				MoggCrypt.swap(ref buf32, 1, 17);
				MoggCrypt.swap(ref buf32, 0, 31);
				MoggCrypt.swap(ref buf32, 29, 2);
				MoggCrypt.swap(ref buf32, 28, 22);
				MoggCrypt.swap(ref buf32, 25, 6);
				MoggCrypt.swap(ref buf32, 24, 26);
				MoggCrypt.swap(ref buf32, 21, 10);
				MoggCrypt.swap(ref buf32, 20, 30);
				MoggCrypt.swap(ref buf32, 17, 14);
				MoggCrypt.swap(ref buf32, 16, 2);
				MoggCrypt.swap(ref buf32, 13, 18);
				MoggCrypt.swap(ref buf32, 12, 6);
				MoggCrypt.swap(ref buf32, 9, 22);
				MoggCrypt.swap(ref buf32, 8, 10);
				MoggCrypt.swap(ref buf32, 5, 26);
				MoggCrypt.swap(ref buf32, 4, 14);
				MoggCrypt.swap(ref buf32, 1, 30);
				MoggCrypt.swap(ref buf32, 0, 18);
			}
			for (int j = 0; j < 32; j++)
			{
				ref byte numPointer = ref buf32[j];
				numPointer = (byte)(numPointer ^ transform[j]);
			}
			return buf32;
		}

		private static byte rotl(byte b, int dist)
		{
			return (byte)((b << (dist & 31) | b >> (8 - dist & 31)) & 255);
		}

		private static byte rotr(byte b, int dist)
		{
			return (byte)((b >> (dist & 31) | b << (8 - dist & 31)) & 255);
		}

		private static void swap(ref byte[] bytes, int i1, int i2)
		{
			byte num = bytes[i1];
			bytes[i1] = bytes[i2];
			bytes[i2] = num;
		}

		private class VorbisCryptStream : Stream
		{
			private Stream mogg;

			private long position;

			private int version;

			private int headerLength;

			private int numEntries;

			private byte[] key;

			private byte[] counter;

			private byte[] initialCounter;

			private byte[] first32;

			private long counterIncrements;

			public override bool CanRead
			{
				get
				{
					return true;
				}
			}

			public override bool CanSeek
			{
				get
				{
					return true;
				}
			}

			public override bool CanWrite
			{
				get
				{
					return false;
				}
			}

			public override long Length
			{
				get;
			}

			public override long Position
			{
				get
				{
					return this.position;
				}
				set
				{
					this.Seek(value, SeekOrigin.Begin);
				}
			}

			public VorbisCryptStream(Stream mogg)
			{
				this.mogg = mogg;
				this.readHeader();
				this.Length = mogg.Length - (long)this.headerLength;
				this.Seek((long)0, SeekOrigin.Begin);
			}

			private void decryptBytes(byte[] target, int offset, int count)
			{
				this.mogg.Position = this.position + (long)this.headerLength;
				if ((this.position - (long)1) / (long)16 != this.counterIncrements)
				{
					this.fixCounter();
				}
				byte[] numArray = this.mogg.ReadBytes(count);
				byte[] numArray1 = new byte[16];
				int num = (int)(this.position % (long)16);
				using (AesManaged aesManaged = new AesManaged())
				{
					aesManaged.Mode = CipherMode.ECB;
					aesManaged.BlockSize = 128;
					aesManaged.KeySize = 128;
					aesManaged.Padding = PaddingMode.None;
					aesManaged.Key = this.key;
					ICryptoTransform cryptoTransform = aesManaged.CreateEncryptor();
					cryptoTransform.TransformBlock(this.counter, 0, (int)this.counter.Length, numArray1, 0);
					for (int i = 0; i < count; i++)
					{
						if (this.position != 0 && this.position % (long)16 == 0)
						{
							int num1 = 0;
							for (int j = 0; j < 16; j++)
							{
								ref byte numPointer = ref this.counter[num1];
								numPointer = (byte)(numPointer + 1);
								if (this.counter[num1] != 0)
								{
									break;
								}
								num1++;
							}
							this.counterIncrements += (long)1;
							num = 0;
							cryptoTransform.TransformBlock(this.counter, 0, (int)this.counter.Length, numArray1, 0);
						}
						target[i + offset] = (byte)(numArray[i] ^ numArray1[num]);
						this.position += (long)1;
						num++;
					}
				}
			}

			private void fixCounter()
			{
				this.initialCounter.CopyTo(this.counter, 0);
				long num = (long)0;
				num = (this.position - (long)1) / (long)16;
				this.counterIncrements = num;
				for (int i = 0; (long)i < num; i++)
				{
					int num1 = 0;
					for (int j = 0; j < 16; j++)
					{
						ref byte numPointer = ref this.counter[num1];
						numPointer = (byte)(numPointer + 1);
						if (this.counter[num1] != 0)
						{
							break;
						}
						num1++;
					}
				}
			}

			public override void Flush()
			{
				throw new NotImplementedException();
			}

			public override int Read(byte[] buffer, int offset, int count)
			{
				int num = 0;
				if (this.position < (long)32)
				{
					while (this.position < (long)32 && count > 0 && offset < (int)buffer.Length)
					{
						buffer[offset] = this.first32[checked((int)(IntPtr)this.position)];
						offset++;
						count--;
						this.position += (long)1;
						num++;
					}
					this.mogg.Position = (long)(32 + this.headerLength);
					this.initialCounter.CopyTo(this.counter, 0);
					ref byte numPointer = ref this.counter[0];
					numPointer = (byte)(numPointer + 1);
					this.counterIncrements = (long)1;
				}
				if (this.position + (long)count > this.Length)
				{
					count = (int)(this.Length - this.position);
				}
				num += count;
				this.decryptBytes(buffer, offset, count);
				return num;
			}

			private void readHeader()
			{
				this.mogg.Position = (long)0;
				this.version = this.mogg.ReadInt32LE();
				if (this.version > 16)
				{
					throw new InvalidDataException("Unrecognized header");
				}
				this.headerLength = this.mogg.ReadInt32LE();
				this.mogg.Position = (long)16;
				this.numEntries = this.mogg.ReadInt32LE();
				this.mogg.Position = (long)(20 + this.numEntries * 8);
				this.counter = this.mogg.ReadBytes(16);
				this.initialCounter = this.counter.Clone() as byte[];
				switch (this.version)
				{
					case 11:
					{
						this.key = MoggCrypt.ctr_key_0B;
						break;
					}
					case 12:
					case 13:
					{
						this.key = MoggCrypt.gen_key(MoggCrypt.HvKey0C, this.mogg, 12);
						break;
					}
					case 14:
					{
						this.key = MoggCrypt.gen_key(MoggCrypt.HvKey0E, this.mogg, 14);
						break;
					}
					case 15:
					{
						this.key = MoggCrypt.gen_key(MoggCrypt.HvKey0F, this.mogg, 15);
						break;
					}
					case 16:
					{
						this.key = MoggCrypt.gen_key(MoggCrypt.HvKey10, this.mogg, 16);
						break;
					}
					case 17:
					{
						this.key = MoggCrypt.gen_key(MoggCrypt.HvKey10, this.mogg, 17);
						break;
					}
				}
				this.first32 = new byte[32];
				this.decryptBytes(this.first32, 0, 32);
				if (this.first32[0] == 72)
				{
					this.mogg.Position = (long)(20 + this.numEntries * 8 + 16);
					int num = this.mogg.ReadInt32LE();
					this.mogg.Position = (long)(20 + this.numEntries * 8 + 16 + 8);
					int num1 = this.mogg.ReadInt32LE();
					int num2 = 1664525 * (1664525 * (num ^ 1549556828) + 1013904223) + 1013904223;
					int num3 = 1664525 * (num1 ^ 909522486) + 1013904223;
					num2 = (int)MoggCrypt.flip_endianness((uint)num2);
					num3 = (int)MoggCrypt.flip_endianness((uint)num3);
					BytetoIntConverter bytetoIntConverter = new BytetoIntConverter()
					{
						asBytes = this.first32
					};
					bytetoIntConverter.asInts[0] = 1399285583;
					bytetoIntConverter.asInts[3] ^= num2;
					bytetoIntConverter.asInts[5] ^= num3;
				}
			}

			public override long Seek(long offset, SeekOrigin origin)
			{
				switch (origin)
				{
					case SeekOrigin.Begin:
					{
						if (offset > this.Length)
						{
							offset = this.Length;
						}
						this.position = offset;
						break;
					}
					case SeekOrigin.Current:
					{
						if (offset + this.position > this.Length)
						{
							offset = (long)0;
						}
						this.position += offset;
						break;
					}
					case SeekOrigin.End:
					{
						if (this.Length + this.position > this.Length)
						{
							offset = (long)0;
						}
						this.position = this.Length + offset;
						break;
					}
				}
				this.fixCounter();
				this.mogg.Position = this.position + (long)this.headerLength;
				return this.position;
			}

			public override void SetLength(long value)
			{
				throw new NotImplementedException();
			}

			public override void Write(byte[] buffer, int offset, int count)
			{
				throw new NotImplementedException();
			}
		}
	}
}