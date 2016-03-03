using RecuperadorXML;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace RecuperadorXML
{
    public partial class Form1 : Form
    {
        private string imgUrl = "";
        private XMLTool xmlt;
        private Utility util;
        private string nfeUrl = "http://www.nfe.fazenda.gov.br/portal/consulta.aspx?tipoConsulta=completa&tipoConteudo=XbSeqxE8pl8=";
        //private string ConsultaCompleta = "http://www.nfe.fazenda.gov.br/portal/consultaCompleta.aspx?tipoConteudo=XbSeqxE8pl8=";

        public Form1()
        {
            InitializeComponent();
        }

        private void Load_Captcha(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
          
            if (page.ReadyState == WebBrowserReadyState.Complete)
            {
                string captcha, captcha1;
                HtmlDocument htmldoc = page.Document;
                HtmlElementCollection htmlElementCollection = htmldoc.All;
                foreach (HtmlElement ele in htmlElementCollection)
                {
                    if (ele.Id == "ctl00_ContentPlaceHolder1_imgCaptcha")
                    {
                        captcha1 = ele.OuterHtml;
                        captcha = captcha1.Substring(177, captcha1.Length - 177);
                        imgUrl = captcha.Substring(0, captcha.Length - 2);
                        pictureBox1.Image = util.Base64ToImage(imgUrl);

                    }
                }

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            page.Navigate(nfeUrl);
            xmlt = new XMLTool(page);
            util = new Utility(page, nfeUrl);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            util.ClearScr(label2, textBox1, textBox2);
            util.GetCaptcha();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            util.EnterSite();
            util.CheckKey(textBox1);
            util.CheckCaptcha(textBox2);
            page.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(xmlt.DownloadXML);
            
            
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

        private void button3_Click(object sender, EventArgs e)
        {
            xmlt.RenameXml();
        }
    }
}
