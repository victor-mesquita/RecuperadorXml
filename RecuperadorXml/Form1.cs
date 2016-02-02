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

        private void Load_Captcha(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            GetCaptcha();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            page.Navigate(nfeUrl);
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            page.Refresh();

           // clearScr();

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
            if(ack != null && ack.Length > 0)
            {

            }
            else
            {
                label2.ForeColor = Color.Red;
                label2.Text = "Chave de Acesso inválida!";

            }
        }

        private void clearScr()
        {
            if(label2.Text != "Digite o valor ao lado")
            {
                label2.ForeColor = Color.Black;
                label2.Text = "Digite o valor ao lado";
            }

            if (pictureBox1.Image != null)
            {
                pictureBox1.Image.Dispose();
                pictureBox1.Image = null;
            }
        }

        private void downloadXml()
        {
            HtmlDocument htmldoc = (HtmlDocument)page.Document;
            HtmlElementCollection htmlElementCollection = htmldoc.GetElementsByTagName("input");
            foreach (HtmlElement ele in htmlElementCollection)
            {

                if (ele.GetAttribute("name") == "ctl00$ContentPlaceHolder1$btnConsultar")
                {
                    ele.InvokeMember("javascript:WebForm_DoPostBackWithOptions(new WebForm_PostBackOptions(\"ctl00$ContentPlaceHolder1$btnConsultar\", \"\", true, \"completa\", \"\", false, false))");
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            checkAKey(textBox1.Text);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            HtmlDocument htmldoc = (HtmlDocument)page.Document;
            HtmlElementCollection htmlElementCollection = htmldoc.GetElementsByTagName("input");
            foreach (HtmlElement ele in htmlElementCollection)
            {

                if (ele.GetAttribute("name") == "ctl00$ContentPlaceHolder1$txtChaveAcessoCompleta")
                {
                    ele.SetAttribute("value", textBox1.Text);
                }
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            HtmlDocument htmldoc = (HtmlDocument)page.Document;
            HtmlElementCollection htmlElementCollection = htmldoc.GetElementsByTagName("input");
            foreach (HtmlElement ele in htmlElementCollection)
            {

                if (ele.GetAttribute("name") == "ctl00$ContentPlaceHolder1$txtCaptcha")
                {
                    ele.SetAttribute("value", textBox2.Text);
                }
            }
        }
    }
}
