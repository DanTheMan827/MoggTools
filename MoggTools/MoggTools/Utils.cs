using DtxCS;
using DtxCS.DataTypes;
using GameArchives;
using LibForge.Midi;
using LibForge.SongData;
using LibMoggCrypt;
using LibOrbisPkg.Util;
using MidiCS;
using MoggTools.Properties;
using System;
using System.Collections.Generic;
using System.IO;

namespace MoggTools
{
    internal static class Utils
    {
        public static string CleanFileName(string fn)
        {
            return fn.Replace("\\", "").Replace("/", "").Replace(":", "").Replace("*", "").Replace("?", "").Replace("\"", "").Replace("<", "").Replace(">", "").Replace("|", "");
        }

        public static DataArray ForgeToOldDTA(IDirectory d, string platform)
        {
            string str;
            IFile file = null;
            DataArray dataArray;
            string name = d.Name;

            str = (!d.TryGetFile(string.Concat(name, ".mogg.dta"), out file) ? DTX.FromDtb(d.GetFile(string.Concat(name, ".mogg.dta_dta_", platform)).GetStream()).ToString().Substring(1) : DTX.FromDtaStream(file.GetStream()).ToString().Substring(1));
            using (Stream stream = d.GetFile(string.Concat(name, ".songdta_", platform)).GetStream())
            {
                SongData songDatum = SongDataReader.ReadStream(stream);
                string str1 = (d.Parent.Parent.Name == "uroot" ? "" : string.Concat(platform, "/"));
                dataArray = DTX.FromDtaString(string.Concat(new string[] { "'", songDatum.Shortname, "' ('name' \"", songDatum.Name.Replace("\"", "\\q"), "\")('artist' \"", songDatum.Artist.Replace("\"", "\\q"), "\")('song' ('name' \"", str1, d.Parent.Name, "/", name, "/", name, "\")", str, string.Format("('year_released' {0})", songDatum.OriginalYear), "('album_name' \"", songDatum.AlbumName.Replace("\"", "\\q"), "\")", string.Format("('song_length' {0:0.##})", songDatum.SongLength) }));
            }
            return dataArray;
        }

        public static string GetFileName(this Song s)
        {
            if (!Settings.Default.RenameToArtistSongName)
            {
                return s.ShortName;
            }
            return s.CleanFullName;
        }

        public static List<Song> LoadSongs(DataArray dta, AbstractPackage pkg)
        {
            MidiFile midiFile;
            DataSymbol dataSymbol;
            object obj;
            List<DataNode> dataNodes = new List<DataNode>();
            foreach (DataNode child in dta.Children)
            {
                if (!(child is DataArray))
                {
                    dataNodes.Add(child);
                }
                else
                {
                    DataArray dataArray = ((DataArray)child).Array("fake");
                    if (dataArray == null || !(dataArray.Any(1) == "TRUE"))
                    {
                        if (Settings.Default.ShowTutorialSongs)
                        {
                            continue;
                        }
                        DataArray dataArray1 = (child as DataArray).Array("tutorial");
                        if (dataArray1 != null)
                        {
                            dataSymbol = dataArray1.Symbol(1);
                        }
                        else
                        {
                            dataSymbol = null;
                        }
                        if (dataSymbol != DataSymbol.Symbol("TRUE"))
                        {
                            continue;
                        }
                        dataNodes.Add(child);
                    }
                    else
                    {
                        dataNodes.Add(child);
                    }
                }
            }
            foreach (DataNode dataNode in dataNodes)
            {
                dta.Children.Remove(dataNode);
            }
            dataNodes.Clear();
            dataNodes = null;
            List<Song> songs = new List<Song>(dta.Count);
            for (int i = 0; i < dta.Count; i++)
            {
                try
                {
                    DataArray dataArray2 = dta.Array(i).Array("song");
                    if (dataArray2 != null)
                    {
                        DataArray dataArray3 = dataArray2.Array("name");
                        if (dataArray3 != null)
                        {
                            obj = dataArray3.Any(1);
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
                        DataArray dataArray4 = dta.Array(i).Array("base_filename");
                        if (dataArray4 != null)
                        {
                            obj = dataArray4.Any(1);
                        }
                        else
                        {
                            obj = null;
                        }
                        if (obj == null)
                        {
                            obj = "";
                        }
                    }
                    string str = string.Concat((string)obj, ".mid");
                    midiFile = MidiFileReader.FromBytes(pkg.GetFile(str).GetBytes());
                }
                catch (FileNotFoundException fileNotFoundException)
                {
                    goto Label0;
                }
                songs.Add(new Song(dta.Array(i), midiFile));
            Label0:;
            }
            return songs;
        }

        public static List<Song> LoadSongsForge(IDirectory root, string platform)
        {
            IDirectory directory = null;
            IFile file = null;
            List<Song> songs = new List<Song>();
            string[] strArrays = new string[] { "songs", "songs_download" };
            for (int i = 0; i < strArrays.Length; i++)
            {
                if (root.TryGetDirectory(strArrays[i], out directory))
                {
                    foreach (IDirectory dir in directory.Dirs)
                    {
                        if (dir.Name.StartsWith("_") || dir.Name.StartsWith("tutorial_") || !dir.TryGetFile(string.Concat(dir.Name, ".mogg"), out file))
                        {
                            continue;
                        }
                        DataArray oldDTA = Utils.ForgeToOldDTA(dir, platform);
                        using (Stream stream = dir.GetFile(string.Concat(dir.Name, ".rbmid_", platform)).GetStream())
                        {
                            songs.Add(new Song(oldDTA, RBMidConverter.ToMid(RBMidReader.ReadStream(stream))));
                        }
                    }
                }
            }
            return songs;
        }

        public static void SaveDTA(Song s, string path)
        {
            File.WriteAllText(Path.Combine(path, string.Concat(s.GetFileName(), ".dta")), s.Data.ToString());
        }

        public static MoggCryptResult SaveMogg(Stream f, string outPath, bool decrypt = true)
        {
            MoggCryptResult moggCryptResult;
            byte[] numArray = f.ReadBytes((int)f.Length);
            moggCryptResult = (!decrypt ? MoggCryptResult.SUCCESS : MoggCrypt.nativeDecrypt(numArray));
            File.WriteAllBytes(outPath, numArray);
            return moggCryptResult;
        }

        public static MoggCryptResult SaveMoggToFile(string shortname, string output_path, byte[] moggFile, Action<string> logln, bool dontDecrypt = false)
        {
            MoggCryptResult moggCryptResult = MoggCryptResult.ERR_UNSUPPORTED_ENCRYPTION;
            if (!dontDecrypt)
            {
                logln(string.Concat("Decrypting ", shortname, ".mogg"));
                moggCryptResult = MoggCrypt.nativeDecrypt(moggFile);
                if (moggCryptResult == MoggCryptResult.SUCCESS)
                {
                    moggFile[0] = 10;
                    File.WriteAllBytes(output_path, moggFile);
                    logln(string.Concat("Saved and decrypted mogg to ", output_path));
                }
                else if (moggCryptResult == MoggCryptResult.ERR_ALREADY_DECRYPTED)
                {
                    File.WriteAllBytes(output_path, moggFile);
                    logln(string.Concat("Saved (already decrypted) mogg to ", output_path));
                }
                else if (moggCryptResult != MoggCryptResult.ERR_DECRYPT_FAILED)
                {
                    logln(string.Concat("Could not decrypt ", shortname, ".mogg! (unsupported encryption)"));
                }
                else
                {
                    logln("Error decrypting mogg: supported encryption scheme, but decrypted data was wrong.");
                }
            }
            else
            {
                try
                {
                    File.WriteAllBytes(output_path, moggFile);
                    logln(string.Concat("Saved encrypted mogg to ", output_path));
                }
                catch
                {
                    logln(string.Concat("Couldn't save encrypted mogg to ", output_path));
                }
            }
            return moggCryptResult;
        }

        public static void SaveRpp(Song s, string path)
        {
            File.WriteAllText(Path.Combine(path, string.Concat(s.GetFileName(), ".rpp")), s.MakeReaperProject(Settings.Default.SaveTempoMapInRPP, Settings.Default.SaveMidiDataInRPP));
        }
    }
}