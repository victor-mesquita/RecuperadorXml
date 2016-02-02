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

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        private string imgUrl = "";
        private string nfeUrl = "http://www.nfe.fazenda.gov.br/portal/consulta.aspx?tipoConsulta=completa";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            page.Navigate(nfeUrl);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            page.Refresh();

            GetCaptcha();
           
        }


        //Converte Base64 para imagem
        public Image Base64ToImage(string base64String)
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0,
              imageBytes.Length);

            // Convert byte[] to Image
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = Image.FromStream(ms, true);
            return image;
        }


        //Obtem o captcha
        private void GetCaptcha()
        {
            if (page.ReadyState == WebBrowserReadyState.Complete)
            {
                HtmlDocument htmldoc = (HtmlDocument)page.Document;
                HtmlElementCollection htmlElementCollection = htmldoc.Images;
                foreach (HtmlElement ele in htmlElementCollection)
                {

                    if (ele.Id == "ctl00_ContentPlaceHolder1_imgCaptcha")
                    {
                        imgUrl = ele.GetAttribute("src");
                        imgUrl = imgUrl.Substring(22, imgUrl.Length - 22);
                        pictureBox1.Image = Base64ToImage(imgUrl);

                    }
                }
            }
        }
        private void checkAKey(string ack) {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
