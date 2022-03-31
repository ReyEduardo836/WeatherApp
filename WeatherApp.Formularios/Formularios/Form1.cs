using Newtonsoft.Json;
using RESTCountries.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WeatherApp.Formularios.Formularios
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ObtenerPaisesAsync();
        }

        public async void ObtenerPaisesAsync()
        {
            var all = await RESTCountriesAPI.GetAllCountriesAsync();
            List<string> countries = all.Select(c => c.Name).ToList();
            foreach (string pais in countries)
                cmbPaises.Items.Add(pais);
        }

        private void  btnConfirm_Click(object sender, EventArgs e)
        {
            string pais = cmbPaises.SelectedItem.ToString();
            string token = "d83e25fc5a343c4a236cda331f10b520";
            string url = $"https://api.openweathermap.org/data/2.5/weather?q={pais}&appid={token}";
            dynamic data = GetWeather(url); ;
            if (data == null)
            {
                vaciarCampos();
                return;
            }

            double kelvin = (data.main.temp);
            double celcius = kelvin - 273.15;

            lblTiempo.Text = data.weather[0].description;
            lblTemperatura.Text = celcius.ToString("N2") + " Cº";
            lblHumedad.Text = data.main.humidity + "%";
            string imagen = data.weather[0].icon;
            string ImageUrl = $"http://openweathermap.org/img/w/{imagen}.png";
            getWeatherPicture(ImageUrl);
        }

        private void vaciarCampos()
        {
            lblTiempo.Text = "EL PAIS SELECCIONADO NO SE ENCUENTRA EN LA API";
            lblTemperatura.Text = "-------";
            lblHumedad.Text = "-------";
            pictureBox1.Image = null;
        }

        private dynamic GetWeather(string url)
        {
            try
            {
                WebRequest webRequest = WebRequest.Create(url);
                HttpWebResponse httpWebResponse = null;
                httpWebResponse = (HttpWebResponse)webRequest.GetResponse();

                string result = string.Empty;
                using (Stream stream = httpWebResponse.GetResponseStream())
                {
                    StreamReader streamReader = new StreamReader(stream);
                    result = streamReader.ReadToEnd();
                }
                dynamic data = JsonConvert.DeserializeObject(result);
                return data;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void getWeatherPicture(string url)
        {
            try
            {
                WebClient wc = new WebClient();
                byte[] bytes = wc.DownloadData(url);

                MemoryStream ms = new MemoryStream(bytes);
                Image img = Image.FromStream(ms);

                pictureBox1.Image = img;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
