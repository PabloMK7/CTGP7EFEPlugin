using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CTGP7.Common;

namespace CTGP7.UI
{
    public partial class MissionRichText : UserControl
    {
        [Browsable(true)]
        [Description("Invoked when the selected course changes")]
        public event EventHandler RichTextChanged;
        private bool allowEvent = true;
        public MissionRichText()
        {
            InitializeComponent();
        }

        private byte[] GetColorCode(Color color)
        {
            if (color == Color.White)
            {
                byte[] ret = new byte[1];
                ret[0] = 0x18;
                return ret;
            } else
            {
                byte[] ret = new byte[4];
                ret[0] = 0x1B;
                ret[1] = Math.Max(color.R, (byte)1);
                ret[2] = Math.Max(color.G, (byte)1);
                ret[3] = Math.Max(color.B, (byte)1);
                return ret;
            }            
        }

        private byte[] GetFormatToggleCode(bool toggleBold, bool toggleItalic, bool toggleUnderline)
        {
            byte[] ret = new byte[3];
            ret[0] = 0x11;
            ret[2] = 0x40;
            ret[1] = (byte)(0x80 | ((toggleBold ? 1 : 0) << 0) | ((toggleItalic ? 1 : 0) << 1) | ((toggleUnderline ? 1 : 0) << 2));
            return ret;
        }
        public int RichTextNewLines
        {
            get
            {
                int lines = 0;
                for (int i = 0; i < richTextBox1.Text.Length; i++)
                {
                    if (richTextBox1.Text[i] == '\n')
                        lines++;
                    if (lines >= 3)
                        break;
                }
                return lines;
            }
        }
        public byte[] RichText
        {
            get
            {
                MemoryStream output = new MemoryStream();
                bool isBold = false, isItalic = false, isUnderline = false;
                Color isColor = Color.White;
                int lines = 1;

                using (RichTextBox rtbTemp = new RichTextBox())
                {
                    // Step through the selected text one char at a time    
                    rtbTemp.Rtf = richTextBox1.Rtf;
                    for (int i = 0; i < rtbTemp.Text.Length; ++i)
                    {
                        if (rtbTemp.Text[i] == '\n')
                            lines++;
                        if (lines > 4)
                            break;

                        rtbTemp.Select(i, 1);

                        bool currBold = rtbTemp.SelectionFont.Bold;
                        bool currItalic = rtbTemp.SelectionFont.Italic;
                        bool currUnderline = rtbTemp.SelectionFont.Underline;

                        Color currColor = rtbTemp.SelectionColor;
                        
                        if (isColor != currColor)
                        {
                            byte[] colorCode = GetColorCode(currColor);
                            output.Write(colorCode, 0, colorCode.Length);
                            isColor = currColor;
                        }
                        if (isBold ^ currBold || isItalic ^ currItalic || isUnderline ^ currUnderline)
                        {
                            byte[] formatCode = GetFormatToggleCode(isBold ^ currBold, isItalic ^ currItalic, isUnderline ^ currUnderline);
                            output.Write(formatCode, 0, formatCode.Length);
                            isBold = currBold;
                            isItalic = currItalic;
                            isUnderline = currUnderline;
                        }
                        byte[] character = Encoding.UTF8.GetBytes(new char[] { rtbTemp.Text[i] });
                        output.Write(character, 0, character.Length);
                    }
                }
                if (isBold || isItalic || isUnderline)
                {
                    byte[] formatCode = GetFormatToggleCode(isBold, isItalic, isUnderline);
                    output.Write(formatCode, 0, formatCode.Length);
                }
                if (isColor != Color.White)
                {
                    byte[] colorCode = GetColorCode(Color.White);
                    output.Write(colorCode, 0, colorCode.Length);
                }
                return output.ToArray();
            }
            set
            {
                // Reset state
                allowEvent = false;
                richTextBox1.Clear();
                boldCheck.Checked = false;
                italicCheck.Checked = false;
                underlineCheck.Checked = false;
                colorDialog1.Color = Color.White;
                changeColorButton.BackColor = colorDialog1.Color;
                richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont, 0);
                richTextBox1.SelectionColor = colorDialog1.Color;

                int currPos = 0;
                while (currPos < value.Length)
                {
                    int decodedpoints = ExtendedUTF8.DecodeCodepoint(out uint codepoint, value, currPos);
                    currPos += decodedpoints;

                    if (codepoint == 0x11 && value[currPos + 1] == 0x40)
                    {
                        byte flags = value[currPos];
                        if ((flags & (1 << 0)) != 0)
                        {
                            richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont, richTextBox1.SelectionFont.Style ^ FontStyle.Bold);
                        }
                        if ((flags & (1 << 1)) != 0)
                        {
                            richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont, richTextBox1.SelectionFont.Style ^ FontStyle.Italic);
                        }
                        if ((flags & (1 << 2)) != 0)
                        {
                            richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont, richTextBox1.SelectionFont.Style ^ FontStyle.Underline);
                        }
                        currPos += 2;
                    } else if (codepoint == 0x18)
                    {
                        richTextBox1.SelectionColor = Color.White;
                    } else if (codepoint == 0x1B)
                    {
                        richTextBox1.SelectionColor = Color.FromArgb(value[currPos], value[currPos + 1], value[currPos + 2]);
                        currPos += 3;
                    } else
                    {
                        richTextBox1.AppendText(new string(new char[] { (char)codepoint }));
                    }
                }
                allowEvent = true;
            }
        }

        public void ChangeFontStyle(FontStyle style, bool add)
        {
            //This method should handle cases that occur when multiple fonts/styles are selected
            // Parameters:-
            //  style - eg FontStyle.Bold
            //  add - IF true then add else remove

            // throw error if style isn't: bold, italic, strikeout or underline
            if (style != FontStyle.Bold
                && style != FontStyle.Italic
                && style != FontStyle.Strikeout
                && style != FontStyle.Underline)
                throw new System.InvalidProgramException("Invalid style parameter to ChangeFontStyle");

            int rtb1start = richTextBox1.SelectionStart;
            int len = richTextBox1.SelectionLength;
            int rtbTempStart = 0;

            //if len <= 1 and there is a selection font then just handle and return
            if (len <= 1 && richTextBox1.SelectionFont != null)
            {
                //add or remove style 
                if (add)
                    richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont, richTextBox1.SelectionFont.Style | style);
                else
                    richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont, richTextBox1.SelectionFont.Style & ~style);

                return;
            }

            using (RichTextBox rtbTemp = new RichTextBox())
            {
                // Step through the selected text one char at a time    
                rtbTemp.Rtf = richTextBox1.SelectedRtf;
                for (int i = 0; i < len; ++i)
                {
                    rtbTemp.Select(rtbTempStart + i, 1);

                    //add or remove style 
                    if (add)
                        rtbTemp.SelectionFont = new Font(rtbTemp.SelectionFont, rtbTemp.SelectionFont.Style | style);
                    else
                        rtbTemp.SelectionFont = new Font(rtbTemp.SelectionFont, rtbTemp.SelectionFont.Style & ~style);
                }

                // Replace & reselect
                rtbTemp.Select(rtbTempStart, len);
                richTextBox1.SelectedRtf = rtbTemp.SelectedRtf;
                richTextBox1.Select(rtb1start, len);
            }
            if (len > 0 && allowEvent)
                RichTextChanged?.Invoke(this, new EventArgs());
        }

        public void ChangeFontColor(Color color)
        {
            int rtb1start = richTextBox1.SelectionStart;
            int len = richTextBox1.SelectionLength;
            int rtbTempStart = 0;

            //if len <= 1 and there is a selection font then just handle and return
            if (len <= 1 && richTextBox1.SelectionFont != null)
            {
                
                richTextBox1.SelectionColor = color;

                return;
            }

            using (RichTextBox rtbTemp = new RichTextBox())
            {
                // Step through the selected text one char at a time    
                rtbTemp.Rtf = richTextBox1.SelectedRtf;
                for (int i = 0; i < len; ++i)
                {
                    rtbTemp.Select(rtbTempStart + i, 1);

                    rtbTemp.SelectionColor = color;
                }

                // Replace & reselect
                rtbTemp.Select(rtbTempStart, len);
                richTextBox1.SelectedRtf = rtbTemp.SelectedRtf;
                richTextBox1.Select(rtb1start, len);
            }
            if (len > 0 && allowEvent)
                RichTextChanged?.Invoke(this, new EventArgs());
        }

        private void boldCheck_CheckedChanged(object sender, EventArgs e)
        {
            ChangeFontStyle(FontStyle.Bold, boldCheck.Checked);
            richTextBox1.Focus();
        }
        private void italicCheck_CheckedChanged(object sender, EventArgs e)
        {
            ChangeFontStyle(FontStyle.Italic, italicCheck.Checked);
            richTextBox1.Focus();
        }

        private void underlineCheck_CheckedChanged(object sender, EventArgs e)
        {
            ChangeFontStyle(FontStyle.Underline, underlineCheck.Checked);
            richTextBox1.Focus();
        }

        private void changeColorButton_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                changeColorButton.BackColor = colorDialog1.Color;
                ChangeFontColor(colorDialog1.Color);
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (allowEvent)
                RichTextChanged?.Invoke(this, new EventArgs());
        }
    }
}
