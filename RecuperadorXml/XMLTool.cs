using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace RecupedaroXML
{
    class XMLTool
    {
        private WebBrowser page { get; set; }

        public XMLTool(WebBrowser page)
        {
            this.page = page;
        }

        public void RenameXml()
        {
            string temp = "C:\\asatransf\\temp";
            string[] arquivos = Directory.GetFiles(temp);
            string xNome = "";
            string NF = "";

            foreach (string xmls in arquivos)
            {
                while (Path.GetExtension(xmls) == ".xml" && File.Exists(xmls))
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(xmls); //Carregando o arquivo

                    XmlNodeList fornecedores = xmlDoc.GetElementsByTagName("emit");

                    foreach (XmlNode fornecedor in fornecedores)
                    {
                        string nomeF = fornecedor["xNome"].InnerText;

                        if (nomeF.Equals("Distribuidora de Medicamentos Santa Cruz Ltda Fl.16"))
                        {
                            xNome = "santa";
                        }
                        else
                        {
                            xNome = nomeF.Substring(0, 6).ToLower();
                        }
                    }

                    XmlNodeList notas = xmlDoc.GetElementsByTagName("ide");
                    foreach (XmlNode nota in notas)
                    {
                        NF = nota["nNF"].InnerText;
                    }

                    File.Move(xmls, "C:\\asatransf\\" + xNome + "" + NF + ".xml");
                    MessageBox.Show("Arquivos tratados com sucesso!", "Recuperador de XML", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            if ((Directory.GetFiles(temp) == null || Directory.GetFiles(temp).Length == 0))
            {
                MessageBox.Show("Nenhum arquivo para ser tratado", "Recuperador de XML", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        public void DownloadXML(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            HtmlDocument htmldoc = page.Document;
            HtmlElementCollection elements = htmldoc.GetElementsByTagName("input");

            foreach (HtmlElement element in elements)
            {
                bool downloadBtn = false;

                if (element.Name.Equals("ctl00$ContentPlaceHolder1$btnDownload"))
                {
                    //element.InvokeMember("Click");
                    downloadBtn = true;
                }

                if (downloadBtn)
                {
                    downloadBtn = false;
                    element.InvokeMember("Click");
                    
                }
                    
            }
        }
    }
}
