using RESTCountries.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
    }
}
