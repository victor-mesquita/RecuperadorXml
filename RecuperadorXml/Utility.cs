using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RecuperadorXML
{
    class Utility
    {
        private WebBrowser page { get; set; }
        private string nfeUrl { get; set; }

        public Utility(WebBrowser page, string nfeUrl)
        {
            this.page = page;
        }


        //Converte Base64 para imagem
        public Image Base64ToImage(string base64String)
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0,
              imageBytes.Length);

            // Converte byte[] para Image
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = Image.FromStream(ms, true);
            return image;
        }

        //Obtem o captcha
        public void GetCaptcha()
        {
            string ReloadScript = @"javascript:__doPostBack('ctl00$ContentPlaceHolder1$hlkAlterarCaptcha','')";

            page.Document.InvokeScript("eval", new object[] { ReloadScript });

        }

        //Verifica a chave de acesso(access key/ack)
        public void CheckKey(TextBox ack)
        {

            if (ack.Text.Length < 44 || ack.Text.Length > 44)
            {
                SetError(4);
            }
            if (ack.Text.Length == 0)
            {
                SetError(0);
            }
        }

        //Verifca o codigo de verificação(Captcha/cpt)
        public void CheckCaptcha(TextBox cpt)
        {
            if (cpt.Text.Length == 0)
            {
                SetError(1);
            }
            if (cpt.Text.Length < 6 && cpt.Text.Length > 0)
            {
                SetError(2);
            }
        }


        //Limpa todos os campos e verifica se a url está correta
        public void ClearScr(Label l1, TextBox t1, TextBox t2, PictureBox cpt)
        {

            //Limpa o label de erros
            if (l1.Text != "Digite o valor ao lado")
            {
                l1.ForeColor = Color.Black;
                l1.Text = "Digite o valor ao lado";
            }

            //Verifica a url e retorna para a pagina principal
            if (page.Url.ToString() != nfeUrl)
                page.Navigate(nfeUrl);

            //Verifica se os Textbox estão realmente preenchidos e limpa caso estejam 
            if(t1.Text != "" || t1.Text != "")
            {
                t1.Text = "";
                t1.Text = "";
            }
            
            //Limpa a picturebox e logo em seguida obtem um novo captcha se já disponivel
            if(cpt.Image != null)
            {
                cpt.Dispose();
                cpt.Image = null;
            }

        }

        //Obtem o elemento/button e executa um 'Click'(action)
        //Executa o action do form
        public void EnterSite()
        {
            HtmlDocument htmldoc = page.Document;
            HtmlElementCollection htmlElementCollection = htmldoc.GetElementsByTagName("input");
            foreach (HtmlElement element in htmlElementCollection)
            {
                Console.WriteLine(element.Name);
                if (element.Name.Equals("ctl00$ContentPlaceHolder1$btnConsultar"))
                {
                    element.InvokeMember("Click");
                }
            }
        }

        //Obtem os errors pelos elementos obtidos no cliente
        public void SetError(int errorCode)
        {
            string[] Errors = { "O campo Chave de Acesso é obrigatório.", "O campo Código da Imagem é obrigatório.", "O campo Código da Imagem deve ter 6 caracteres.", "Código da Imagem inválido. Tente novamente.", "Chave de acesso inválida. A chave de acesso deve ter 44 dígitos." };


            HtmlDocument htmldoc = page.Document;
            HtmlElementCollection htmlDivCollection = htmldoc.GetElementsByTagName("div");


            foreach (HtmlElement element in htmlDivCollection)
            {
                if (element.GetAttribute("id") == "ctl00_ContentPlaceHolder1_vdsErros")
                {
                    HtmlElementCollection childLI = element.All;
                    foreach (HtmlElement li in childLI)
                    {

                        if (li.GetElementsByTagName("li") != null)
                        {
                            switch (errorCode)
                            {
                                case 0:
                                    if (li.InnerText == Errors[0])
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
    }
}
