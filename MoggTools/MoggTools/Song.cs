using DtxCS.DataTypes;
using MidiCS;
using MidiCS.Events;
using MoggTools.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace MoggTools
{
	internal class Song
	{
		private string name;

		private string artist;

		private string album;

		private string year;

		private string id;

		private string shortName;

		private string path;

		private double length;

		private MidiFile midi;

		private DataArray dtaData;

		private DataArray tracks;

		private DataArray pans;

		private DataArray vols;

		public string Album
		{
			get
			{
				return this.album;
			}
		}

		public string Artist
		{
			get
			{
				return this.artist;
			}
		}

		public string CleanFullName
		{
			get
			{
				return Utils.CleanFileName(this.FullName);
			}
		}

		public DataArray Data
		{
			get
			{
				return this.dtaData;
			}
		}

		public string Duration
		{
			get
			{
				if (this.length == 0)
				{
					return "??:??";
				}
				int num = (int)(this.length / 60);
				string str = num.ToString("D2");
				num = (int)(this.length % 60);
				return string.Concat(str, ":", num.ToString("D2"));
			}
		}

		public string FullName
		{
			get
			{
				return string.Concat(this.Artist, " - ", this.Name);
			}
		}

		public string Id
		{
			get
			{
				return this.id;
			}
		}

		public double Length
		{
			get
			{
				if (this.length == 0)
				{
					return 600;
				}
				return this.length;
			}
		}

		public string MoggFullFileName
		{
			get
			{
				return string.Concat(this.CleanFullName, ".mogg");
			}
		}

		public string Name
		{
			get
			{
				return this.name;
			}
		}

		public DataArray Pans
		{
			get
			{
				return this.pans;
			}
		}

		public string Path
		{
			get
			{
				return this.path;
			}
		}

		public string ShortName
		{
			get
			{
				return this.shortName;
			}
		}

		public DataArray Tracks
		{
			get
			{
				return this.tracks;
			}
		}

		public DataArray Vols
		{
			get
			{
				return this.vols;
			}
		}

		public string Year
		{
			get
			{
				return this.year;
			}
		}

		public Song(DataArray arr, MidiFile midi)
		{
			object obj;
			DataArray dataArray;
			DataArray dataArray1;
			DataArray dataArray2;
			object obj1;
			object obj2;
			object obj3;
			object obj4;
			object obj5;
			float? nullable;
			this.midi = midi;
			double num = (midi != null ? midi.Duration : 0);
			this.shortName = arr.Any(0);
			DataArray dataArray3 = arr.Array("song");
			if (dataArray3 != null)
			{
				DataArray dataArray4 = dataArray3.Array("name");
				if (dataArray4 != null)
				{
					obj = dataArray4.Any(1);
				}
				else
				{
					obj = null;
				}
			}
			else
			{
				obj = null;
			}
			if (obj == null)
			{
				obj = "";
			}
			this.path = (string)obj;
			DataArray dataArray5 = arr.Array("song");
			if (dataArray5 != null)
			{
				DataArray dataArray6 = dataArray5.Array("tracks");
				if (dataArray6 != null)
				{
					dataArray = dataArray6.Array(1);
				}
				else
				{
					dataArray = null;
				}
			}
			else
			{
				dataArray = null;
			}
			this.tracks = dataArray;
			DataArray dataArray7 = arr.Array("song");
			if (dataArray7 != null)
			{
				DataArray dataArray8 = dataArray7.Array("pans");
				if (dataArray8 != null)
				{
					dataArray1 = dataArray8.Array(1);
				}
				else
				{
					dataArray1 = null;
				}
			}
			else
			{
				dataArray1 = null;
			}
			this.pans = dataArray1;
			DataArray dataArray9 = arr.Array("song");
			if (dataArray9 != null)
			{
				DataArray dataArray10 = dataArray9.Array("vols");
				if (dataArray10 != null)
				{
					dataArray2 = dataArray10.Array(1);
				}
				else
				{
					dataArray2 = null;
				}
			}
			else
			{
				dataArray2 = null;
			}
			this.vols = dataArray2;
			DataArray dataArray11 = arr.Array("name");
			if (dataArray11 != null)
			{
				obj1 = dataArray11.Any(1);
			}
			else
			{
				obj1 = null;
			}
			if (obj1 == null)
			{
				DataArray dataArray12 = arr.Array("title");
				if (dataArray12 != null)
				{
					obj1 = dataArray12.Any(1);
				}
				else
				{
					obj1 = null;
				}
				if (obj1 == null)
				{
					obj1 = this.shortName;
				}
			}
			this.name = (string)obj1;
			DataArray dataArray13 = arr.Array("artist");
			if (dataArray13 != null)
			{
				obj2 = dataArray13.Any(1);
			}
			else
			{
				obj2 = null;
			}
			if (obj2 == null)
			{
				obj2 = "(unspecified)";
			}
			this.artist = (string)obj2;
			DataArray dataArray14 = arr.Array("year_released");
			if (dataArray14 != null)
			{
				obj3 = dataArray14.Any(1);
			}
			else
			{
				obj3 = null;
			}
			if (obj3 == null)
			{
				obj3 = "";
			}
			this.year = (string)obj3;
			DataArray dataArray15 = arr.Array("album_name");
			if (dataArray15 != null)
			{
				obj4 = dataArray15.Any(1);
			}
			else
			{
				obj4 = null;
			}
			if (obj4 == null)
			{
				obj4 = "";
			}
			this.album = (string)obj4;
			DataArray dataArray16 = arr.Array("song_id");
			if (dataArray16 != null)
			{
				obj5 = dataArray16.Any(1);
			}
			else
			{
				obj5 = null;
			}
			if (obj5 == null)
			{
				obj5 = "";
			}
			this.id = (string)obj5;
			if (num != 0)
			{
				this.length = num;
			}
			else
			{
				DataArray dataArray17 = arr.Array("song_length");
				if (dataArray17 != null)
				{
					nullable = new float?(dataArray17.Number(1));
				}
				else
				{
					nullable = null;
				}
				float? nullable1 = nullable;
				this.length = (nullable1.HasValue ? (double)nullable1.GetValueOrDefault() : 0) / 1000;
			}
			this.dtaData = arr;
		}

		private string make_item(int channel, bool stereo)
		{
			int num = 3 + (stereo ? 64 + channel : channel);
			IFormatProvider cultureInfo = new CultureInfo("en-US");
			string vorbisItem = Resources.vorbis_item;
			Guid guid = Guid.NewGuid();
			string str = vorbisItem.Replace("$GUID$", guid.ToString());
			guid = Guid.NewGuid();
			string str1 = str.Replace("$IGUID$", guid.ToString()).Replace("$MOGGNAME$", string.Concat(this.GetFileName(), ".mogg"));
			double num1 = (this.length == 0 ? 600 : this.length);
			return str1.Replace("$LENGTH$", num1.ToString(cultureInfo)).Replace("$CHANMODE$", num.ToString());
		}

		private string make_midi_item(MidiTrack t, int ticksPerQN)
		{
			byte[] metaType;
			StringBuilder stringBuilder = new StringBuilder(25 * t.Messages.Count);
			foreach (IMidiMessage message in t.Messages)
			{
				if (!(message is IMidiEvent))
				{
					if (!(message is MetaEvent))
					{
						continue;
					}
					MetaEvent metaEvent = message as MetaEvent;
					MetaEventType metaEventType = metaEvent.MetaType;
					if (metaEventType > MetaEventType.EndOfTrack)
					{
						if (metaEventType <= MetaEventType.SmtpeOffset)
						{
							if (metaEventType == MetaEventType.TempoEvent || metaEventType == MetaEventType.SmtpeOffset)
							{
								continue;
							}
						}
						else if (metaEventType == MetaEventType.TimeSignature || metaEventType == MetaEventType.KeySignature || metaEventType == MetaEventType.SequencerSpecific)
						{
							continue;
						}
					}
					else if (metaEventType > MetaEventType.CuePoint)
					{
						if (metaEventType == MetaEventType.ChannelPrefix || metaEventType == MetaEventType.EndOfTrack)
						{
							continue;
						}
					}
					else if (metaEventType == MetaEventType.SequenceNumber)
					{
						metaType = new byte[] { 255, (byte)metaEvent.MetaType, (byte)((metaEvent as SequenceNumber).Number >> 8), (byte)((metaEvent as SequenceNumber).Number & 255) };
						stringBuilder.Append("\r\n        ");
						stringBuilder.AppendFormat("<X {0} 0\r\n          ", message.DeltaTime);
						stringBuilder.Append(Convert.ToBase64String(metaType));
						stringBuilder.Append("\r\n        >");
					}
					else if ((byte)metaEventType - (byte)MetaEventType.TextEvent <= (byte)MetaEventType.Marker)
					{
						metaType = new byte[2 + (metaEvent as MetaTextEvent).Text.Length];
						metaType[0] = 255;
						metaType[1] = (byte)metaEvent.MetaType;
						Encoding.ASCII.GetBytes((metaEvent as MetaTextEvent).Text).CopyTo(metaType, 2);
						stringBuilder.Append("\r\n        ");
						stringBuilder.AppendFormat("<X {0} 0\r\n          ", message.DeltaTime);
						stringBuilder.Append(Convert.ToBase64String(metaType));
						stringBuilder.Append("\r\n        >");
					}
				}
				else
				{
					stringBuilder.AppendFormat("\r\n        E {0} {1} ", message.DeltaTime, ((byte)((byte)message.Type + (message as IMidiEvent).Channel)).ToHexString());
					EventType type = message.Type;
					if (type <= EventType.NotePresure)
					{
						if (type == EventType.NoteOff)
						{
							stringBuilder.AppendFormat("{0} {1}", (message as NoteOffEvent).Key.ToHexString(), (message as NoteOffEvent).Velocity.ToHexString());
						}
						else if (type == EventType.NoteOn)
						{
							stringBuilder.AppendFormat("{0} {1}", (message as NoteOnEvent).Key.ToHexString(), (message as NoteOnEvent).Velocity.ToHexString());
						}
						else if (type == EventType.NotePresure)
						{
							stringBuilder.AppendFormat("{0} {1}", (message as NotePressureEvent).Key.ToHexString(), (message as NotePressureEvent).Pressure.ToHexString());
						}
					}
					else if (type <= EventType.ProgramChange)
					{
						if (type == EventType.Controller)
						{
							stringBuilder.AppendFormat("{0} {1}", (message as ControllerEvent).Controller.ToHexString(), (message as ControllerEvent).Value.ToHexString());
						}
						else if (type == EventType.ProgramChange)
						{
							stringBuilder.Append((message as ProgramChgEvent).Program.ToHexString());
						}
					}
					else if (type == EventType.ChannelPressure)
					{
						stringBuilder.Append((message as ChannelPressureEvent).Pressure.ToHexString());
					}
					else if (type == EventType.PitchBend)
					{
						stringBuilder.AppendFormat("{0} {1}", ((byte)((message as PitchBendEvent).Bend >> 7)).ToHexString(), ((byte)((message as PitchBendEvent).Bend & 63)).ToHexString());
					}
				}
			}
			IFormatProvider cultureInfo = new CultureInfo("en-US");
			string midiItem = Resources.midi_item;
			Guid guid = Guid.NewGuid();
			string str = midiItem.Replace("$GUID$", guid.ToString());
			guid = Guid.NewGuid();
			string str1 = str.Replace("$IGUID$", guid.ToString()).Replace("$NAME$", t.Name);
			double length = this.Length;
			string str2 = str1.Replace("$LENGTH$", length.ToString(cultureInfo)).Replace("$TICKSPERQN$", ticksPerQN.ToString());
			guid = Guid.NewGuid();
			return str2.Replace("$MIDIGUID$", guid.ToString()).Replace("$EVENTS$", stringBuilder.ToString());
		}

		private string make_midi_track(MidiTrack t)
		{
			if (t == null)
			{
				return "";
			}
			float single = 1f;
			float single1 = 0f;
			IFormatProvider cultureInfo = new CultureInfo("en-US");
			string rppTrack = Resources.rpp_track;
			Guid guid = Guid.NewGuid();
			return rppTrack.Replace("$GUID$", guid.ToString()).Replace("$NAME$", t.Name).Replace("$VOL$", single.ToString(cultureInfo)).Replace("$PAN$", single1.ToString(cultureInfo)).Replace("$ITEM$", this.make_midi_item(t, (int)this.midi.TicksPerQN));
		}

		private string make_tempomap()
		{
			string str = "";
			foreach (TimeSigTempoEvent tempoTimeSigMap in this.midi.TempoTimeSigMap)
			{
				string[] strArrays = new string[] { str, "\r\n    PT ", null, null, null, null, null };
				double time = tempoTimeSigMap.Time;
				strArrays[2] = time.ToString("F13");
				strArrays[3] = " ";
				time = tempoTimeSigMap.BPM;
				strArrays[4] = time.ToString("F10");
				strArrays[5] = " 1";
				strArrays[6] = (tempoTimeSigMap.NewTimeSig ? string.Concat(" ", (tempoTimeSigMap.Denominator << 16) + tempoTimeSigMap.Numerator) : "");
				str = string.Concat(strArrays);
			}
			return str;
		}

		private string make_track(int channel, bool stereo, string name)
		{
			float single = this.voldb(this.vols.Float(channel));
			float single1 = (stereo ? 0f : this.pans.Float(channel));
			IFormatProvider cultureInfo = new CultureInfo("en-US");
			string rppTrack = Resources.rpp_track;
			Guid guid = Guid.NewGuid();
			return rppTrack.Replace("$GUID$", guid.ToString()).Replace("$NAME$", name).Replace("$VOL$", single.ToString(cultureInfo)).Replace("$PAN$", single1.ToString(cultureInfo)).Replace("$ITEM$", this.make_item(channel, stereo));
		}

		private string make_tracks(bool saveMidiData = false)
		{
			float[] num = new float[this.pans.Count];
			int num1 = 0;
			foreach (DataAtom child in this.pans.Children)
			{
				int num2 = num1;
				num1 = num2 + 1;
				num[num2] = child.Float;
			}
			int num3 = 0;
			string str = "";
			DataArray dataArray = this.tracks.Array("drum");
			DataArray dataArray1 = this.tracks.Array("bass");
			DataArray dataArray2 = this.tracks.Array("guitar");
			DataArray dataArray3 = this.tracks.Array("vocals");
			DataArray dataArray4 = this.tracks.Array("keys");
			if (dataArray != null)
			{
				if (saveMidiData)
				{
					str = string.Concat(str, this.make_midi_track(this.midi.GetTrackByName("PART DRUMS")));
				}
				switch (dataArray.Array(1).Count)
				{
					case 1:
					{
						str = string.Concat(str, this.make_track(0, false, "Drums"));
						num3++;
						break;
					}
					case 2:
					{
						str = string.Concat(str, this.make_track(0, true, "Drums"));
						num3 += 2;
						break;
					}
					case 3:
					{
						str = string.Concat(str, this.make_track(0, false, "Kick"));
						str = string.Concat(str, this.make_track(1, true, "Kit"));
						num3 += 3;
						break;
					}
					case 4:
					{
						if ((double)num[0] != 0)
						{
							str = string.Concat(str, this.make_track(0, true, "Kick"));
						}
						else
						{
							str = string.Concat(str, this.make_track(0, false, "Kick"));
							str = string.Concat(str, this.make_track(1, false, "Snare"));
						}
						str = string.Concat(str, this.make_track(2, true, "Kit"));
						num3 += 4;
						break;
					}
					case 5:
					{
						str = string.Concat(str, this.make_track(0, false, "Kick"));
						str = string.Concat(str, this.make_track(1, true, "Snare"));
						str = string.Concat(str, this.make_track(3, true, "Kit"));
						num3 += 5;
						break;
					}
					case 6:
					{
						str = string.Concat(str, this.make_track(0, true, "Kick"));
						str = string.Concat(str, this.make_track(2, true, "Snare"));
						str = string.Concat(str, this.make_track(4, true, "Kit"));
						num3 += 6;
						break;
					}
				}
			}
			if (dataArray1 != null)
			{
				if (saveMidiData)
				{
					str = string.Concat(str, this.make_midi_track(this.midi.GetTrackByName("PART BASS")));
				}
				if (dataArray1.Children[1] is DataAtom || dataArray1.Array(1).Count == 1)
				{
					int num4 = num3;
					num3 = num4 + 1;
					str = string.Concat(str, this.make_track(num4, false, "Bass"));
				}
				else
				{
					str = string.Concat(str, this.make_track(num3, true, "Bass"));
					num3 += 2;
				}
			}
			if (dataArray2 != null)
			{
				if (saveMidiData)
				{
					str = string.Concat(str, this.make_midi_track(this.midi.GetTrackByName("PART GUITAR")));
				}
				if (dataArray2.Children[1] is DataAtom || dataArray2.Array(1).Count == 1)
				{
					int num5 = num3;
					num3 = num5 + 1;
					str = string.Concat(str, this.make_track(num5, false, "Guitar"));
				}
				else
				{
					str = string.Concat(str, this.make_track(num3, true, "Guitar"));
					num3 += 2;
				}
			}
			if (dataArray3 != null)
			{
				if (saveMidiData)
				{
					str = string.Concat(str, this.make_midi_track(this.midi.GetTrackByName("PART VOCALS")));
					str = string.Concat(str, this.make_midi_track(this.midi.GetTrackByName("HARM1")));
					str = string.Concat(str, this.make_midi_track(this.midi.GetTrackByName("HARM2")));
					str = string.Concat(str, this.make_midi_track(this.midi.GetTrackByName("HARM3")));
				}
				if (dataArray3.Children[1] is DataAtom || dataArray3.Array(1).Count == 1)
				{
					int num6 = num3;
					num3 = num6 + 1;
					str = string.Concat(str, this.make_track(num6, false, "Vocals"));
				}
				else
				{
					str = string.Concat(str, this.make_track(num3, true, "Vocals"));
					num3 += 2;
				}
			}
			if (dataArray4 != null)
			{
				if (saveMidiData)
				{
					str = string.Concat(str, this.make_midi_track(this.midi.GetTrackByName("PART KEYS")));
					str = string.Concat(str, this.make_midi_track(this.midi.GetTrackByName("PART REAL_KEYS_X")));
					str = string.Concat(str, this.make_midi_track(this.midi.GetTrackByName("PART REAL_KEYS_H")));
					str = string.Concat(str, this.make_midi_track(this.midi.GetTrackByName("PART REAL_KEYS_M")));
					str = string.Concat(str, this.make_midi_track(this.midi.GetTrackByName("PART REAL_KEYS_E")));
					str = string.Concat(str, this.make_midi_track(this.midi.GetTrackByName("PART REAL_KEYS_ANIM_RH")));
					str = string.Concat(str, this.make_midi_track(this.midi.GetTrackByName("PART REAL_KEYS_ANIM_LH")));
				}
				if (dataArray4.Children[1] is DataAtom || dataArray4.Array(1).Count == 1)
				{
					int num7 = num3;
					num3 = num7 + 1;
					str = string.Concat(str, this.make_track(num7, false, "Keys"));
				}
				else if (dataArray4.Array(1).Count == 2)
				{
					str = string.Concat(str, this.make_track(num3, true, "Keys"));
					num3 += 2;
				}
			}
			switch ((int)num.Length - num3)
			{
				case 1:
				{
					str = string.Concat(str, this.make_track(num3, false, "Additional Instruments"));
					break;
				}
				case 2:
				{
					str = string.Concat(str, this.make_track(num3, true, "Additional Instruments"));
					break;
				}
				case 3:
				{
					int num8 = num3;
					num3 = num8 + 1;
					str = string.Concat(str, this.make_track(num8, false, "Additional Instruments"));
					str = string.Concat(str, this.make_track(num3, true, "Audience"));
					break;
				}
				case 4:
				{
					str = string.Concat(str, this.make_track(num3, true, "Additional Instruments"));
					str = string.Concat(str, this.make_track(num3 + 2, true, "Audience"));
					break;
				}
			}
			if (saveMidiData)
			{
				str = string.Concat(str, this.make_midi_track(this.midi.GetTrackByName("VR EVENTS")));
				str = string.Concat(str, this.make_midi_track(this.midi.GetTrackByName("EVENTS")));
				str = string.Concat(str, this.make_midi_track(this.midi.GetTrackByName("BEAT")));
				str = string.Concat(str, this.make_midi_track(this.midi.GetTrackByName("VENUE")));
				str = string.Concat(str, this.make_midi_track(this.midi.GetTrackByName("MARKUP")));
			}
			return str;
		}

		public string MakeReaperProject(bool saveTempoMap = true, bool saveMidiData = false)
		{
			if (this.midi == null)
			{
				saveTempoMap = false;
				saveMidiData = false;
			}
			string rppProj = Resources.rpp_proj;
			string str = this.make_tracks(saveMidiData);
			return rppProj.Replace("$TEMPOMAP$", (saveTempoMap ? this.make_tempomap() : "")).Replace("$TRACKS$", str);
		}

		public override string ToString()
		{
			return this.Name;
		}

		private float voldb(float db)
		{
			return (float)Math.Pow(10, (double)(db / 20f));
		}
	}
}