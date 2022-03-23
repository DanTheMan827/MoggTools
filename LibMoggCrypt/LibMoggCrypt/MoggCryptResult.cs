using System;

namespace LibMoggCrypt
{
	public enum MoggCryptResult
	{
		ERR_DECRYPT_FAILED = -3,
		ERR_ALREADY_DECRYPTED = -2,
		ERR_UNSUPPORTED_ENCRYPTION = -1,
		SUCCESS = 0
	}
}