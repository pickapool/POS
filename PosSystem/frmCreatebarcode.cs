using SkiaSharp;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ZXing;

namespace PosSystem
{
    public partial class frmCreatebarcode : Form
    {
        public frmCreatebarcode()
        {
            InitializeComponent();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        // Inside the btnGenerate_Click method, ensure BarcodeFormat is referenced correctly
        private void btnGenerate_Click(object sender, EventArgs e)
        {
            string barcodeText = txtBarcod.Text.Trim();

            // Validate barcode length and content
            if (barcodeText.Length < 11 || barcodeText.Length > 12 || !long.TryParse(barcodeText, out _))
            {
                MessageBox.Show("Barcode must be 11 or 12 numeric characters.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (barcodeText.Length >= 12 && barcodeText.All(char.IsDigit))
            {
                var barcodeWriter = new BarcodeWriter
                {
                    Format = ZXing.BarcodeFormat.UPC_A,
                    Options = new ZXing.Common.EncodingOptions
                    {
                        Width = 100,
                        Height = 50,
                        Margin = 0
                    }
                };

                using (var bitmap = barcodeWriter.Write(barcodeText))
                {
                    using (var ms = new MemoryStream())
                    {
                        bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        pictureBox.Image = new Bitmap(ms);
                    }

                    this.dataSet11.Clear();

                    using (var ms = new MemoryStream())
                    {
                        bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        byte[] imageBytes = ms.ToArray();
                        for (int i = 0; i < number.Value; i++)
                        {
                            this.dataSet11.dtBarcode.AdddtBarcodeRow(txtBarcod.Text, imageBytes);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please provide a valid 12-digit UPC-A barcode text.");
            }

            using (frmPrint FRM = new frmPrint(this.dataSet11.dtBarcode))
            {
                FRM.ShowDialog();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void frmCreatebarcode_Load(object sender, EventArgs e)
        {

        }
    }
}
