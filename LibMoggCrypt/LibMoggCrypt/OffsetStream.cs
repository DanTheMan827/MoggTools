using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace LibMoggCrypt
{
	public class OffsetStream : Stream
	{
		private Stream pkg;

		private long data_offset;

		private long _position;

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
				return this._position;
			}
			set
			{
				this.Seek(value, SeekOrigin.Begin);
			}
		}

		public OffsetStream(Stream package, long offset, long length)
		{
			this.pkg = package;
			this.data_offset = offset;
			this.Length = length;
		}

		public override void Flush()
		{
			throw new NotSupportedException();
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			this.pkg.Seek(this.data_offset + this.Position, SeekOrigin.Begin);
			if ((long)count + this.Position > this.Length)
			{
				count = (int)(this.Length - this.Position);
			}
			int num = this.pkg.Read(buffer, offset, count);
			this._position += (long)num;
			return num;
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
					else if (offset < (long)0)
					{
						offset = (long)0;
					}
					this._position = offset;
					return this._position;
				}
				case SeekOrigin.Current:
				{
					offset += this._position;
					if (offset > this.Length)
					{
						offset = this.Length;
					}
					else if (offset < (long)0)
					{
						offset = (long)0;
					}
					this._position = offset;
					return this._position;
				}
				case SeekOrigin.End:
				{
					offset += this.Length;
					if (offset > this.Length)
					{
						offset = this.Length;
					}
					else if (offset < (long)0)
					{
						offset = (long)0;
					}
					this._position = offset;
					return this._position;
				}
				default:
				{
					if (offset > this.Length)
					{
						offset = this.Length;
					}
					else if (offset < (long)0)
					{
						offset = (long)0;
					}
					this._position = offset;
					return this._position;
				}
			}
		}

		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}
	}
}