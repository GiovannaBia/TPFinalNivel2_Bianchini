using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio;
using Negocio;

namespace WindowsFormsApp1
{
    public partial class frmDetalles : Form
    {

        //Constructor para pasarle por parametro cada atributo del objeto seleccionado.

        public frmDetalles(string codigo, string nombre, string descripcion, string marca, string categoria, string precio, string imagenUrl)
        {
            InitializeComponent();

            txtCodigoDetalles.Text = codigo;
            txtNombreDetalles.Text = nombre;
            txtDescripcionDetalles.Text = descripcion;
            txtMarcaDetalles.Text = marca;
            txtCategoriaDetalles.Text = categoria;
            txtPrecioDetalles.Text = precio;
            try
            {
                pbxDetalles.Load(imagenUrl);
            }
            catch (Exception)
            {

                pbxDetalles.Load("https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRKZ1SMO3FUQBP7gzSU3d1Rr1SqSIQzqKdqVA&usqp=CAU");
            }

            


        }

        private void btnSalirDetalles_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}

