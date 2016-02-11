using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        private string imgUrl = "";
        private bool afterDownload = false;
        private string nfeUrl = "http://www.nfe.fazenda.gov.br/portal/consulta.aspx?tipoConsulta=completa&tipoConteudo=XbSeqxE8pl8=";
        public Form1()
        {
            InitializeComponent();
        }

        private void Load_Captcha(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (page.ReadyState == WebBrowserReadyState.Complete)
            {
                HtmlDocument htmldoc = page.Document;
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

        private void Form1_Load(object sender, EventArgs e)
        {
            page.Navigate(nfeUrl);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            clearScr();
            GetCaptcha();
            afterDownload = false;
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
            /*
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

            }*/
            string ReloadScript = @"javascript:__doPostBack('ctl00$ContentPlaceHolder1$hlkAlterarCaptcha','')";

            page.Document.InvokeScript("eval", new object[] { ReloadScript });

        }
        private void checkKey(TextBox ack) {

            if (ack.Text.Length < 44 || ack.Text.Length > 44)
            {
                SetError(4);
            }
            if(ack.Text.Length == 0)
            {
                SetError(0);
            }
        }

        private void checkCaptcha(TextBox cpt)
        {
            if(cpt.Text.Length == 0)
            {
                SetError(1);
            }
            if(cpt.Text.Length < 6 && cpt.Text.Length > 0)
            {
                SetError(2);
            }
        }

        private void clearScr()
        {
            if(label2.Text != "Digite o valor ao lado")
            {
                label2.ForeColor = Color.Black;
                label2.Text = "Digite o valor ao lado";
            }

            if(page.Url.ToString() != nfeUrl)
                page.Navigate(nfeUrl);

            
        }

        private void EnterSite()
        {
            HtmlDocument htmldoc = page.Document;
            HtmlElementCollection htmlElementCollection = htmldoc.GetElementsByTagName("input");
            foreach (HtmlElement element in htmlElementCollection)
            {
                /*if(ele.GetAttribute("name") == "ctl00$ContentPlaceHolder1$btnConsultar")
                {
                    ele.InvokeMember("submit");
                }*/
                Console.WriteLine(element.Name);
                if (element.Name.Equals("ctl00$ContentPlaceHolder1$btnConsultar")){
                    element.InvokeMember("Click");
                }
            }
            //Em testes
            /*string enterScript = @"javascript:WebForm_DoPostBackWithOptions(new WebForm_PostBackOptions('ctl00$ContentPlaceHolder1$btnConsultar', '', true, 'completa', '', false, false))";
            page.Document.InvokeScript("eval", new object[] { enterScript });*/
        }

        private void SetError(int errorCode)
        {
            string[] Errors = { "O campo Chave de Acesso é obrigatório.", "O campo Código da Imagem é obrigatório.", "O campo Código da Imagem deve ter 6 caracteres.", "Código da Imagem inválido. Tente novamente.", "Chave de acesso inválida. A chave de acesso deve ter 44 dígitos." };


            HtmlDocument htmldoc = page.Document;
            HtmlElementCollection htmlDivCollection =  htmldoc.GetElementsByTagName("div");
            

            foreach(HtmlElement element in htmlDivCollection)
            {
              if (element.GetAttribute("id") == "ctl00_ContentPlaceHolder1_vdsErros")
                {
                    HtmlElementCollection childLI = element.All;
                    foreach(HtmlElement li in childLI)
                    {

                        if(li.GetElementsByTagName("li") != null)
                        {
                            switch (errorCode)
                            {
                                case 0:
                                    if(li.InnerText == Errors[0])
                                    {
                                        MessageBox.Show(Errors[0]);
                                    }
                                    break;
                                case 1:
                                    if (li.InnerText == Errors[1])
                                    {
                                        MessageBox.Show(Errors[1]);
                                    }
                                    break;
                                case 2:
                                    if (li.InnerText == Errors[2])
                                    {
                                        MessageBox.Show(Errors[2]);
                                    }
                                    break;
                                case 4:
                                    if (li.InnerText == Errors[4])
                                    {
                                        MessageBox.Show(Errors[4]);
                                    }
                                    break;
                            }

                        } 
                    }
                }
            }
        }

        private void DownloadXML(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            HtmlDocument htmldoc = page.Document;
                HtmlElementCollection elements = htmldoc.All;

                foreach (HtmlElement element in elements)
                {
                if (element.Name.Equals("ctl00$ContentPlaceHolder1$btnDownload"))
                    element.InvokeMember("Click");
                    afterDownload = true;
                }
        }

        private void RenameXml()
        {
            string[] arquivos = Directory.GetFiles("C:\\asatransf\\temp");
            string xNome = "";
            string NF = "";

            foreach(string xmls in arquivos)
            {
                 while (Path.GetExtension(xmls) == ".xml" && File.Exists(xmls))
                    {
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.Load(xmls); //Carregando o arquivo

                        XmlNodeList fornecedores = xmlDoc.GetElementsByTagName("emit");

                        foreach(XmlNode fornecedor in fornecedores)
                        {
                        string nomeF = fornecedor["xNome"].InnerText;

                        if(nomeF.Equals("Distribuidora de Medicamentos Santa Cruz Ltda Fl.16"))
                        {
                            xNome = "santa";
                        }
                        else
                        {
                            xNome = nomeF.Substring(0, 6).ToLower();
                        }
                        }

                        XmlNodeList notas = xmlDoc.GetElementsByTagName("ide");
                           foreach(XmlNode nota in notas)
                        {
                            NF = nota["nNF"].InnerText;
                        }

                    File.Move(xmls, "C:\\asatransf\\" + xNome + "" + NF+".xml");
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
            EnterSite();
            checkKey(textBox1);
            checkCaptcha(textBox2);
            page.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(DownloadXML);
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
            RenameXml();
        }
    }
}
