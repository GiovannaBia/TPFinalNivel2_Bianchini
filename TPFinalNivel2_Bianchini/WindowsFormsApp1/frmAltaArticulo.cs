using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Negocio;
using Dominio;
using System.IO;

namespace WindowsFormsApp1
{
    public partial class frmAltaArticulo : Form
    {
        private Articulo articulo = null;
        private OpenFileDialog archivo = null;
        public frmAltaArticulo()
        {
            InitializeComponent();   //Si no le paso un art por parametro, queda en null, es Agregar
        }

        public frmAltaArticulo(Articulo articulo)
        {
            InitializeComponent();    //le paso articulo, ya no es nulo, va a la funcionalidad Modificar
            this.articulo = articulo;
            Text = "Modificar Artículo";
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }
     
        private void frmAltaArticulo_Load(object sender, EventArgs e)
        {

            MarcaNegocio marcaNegocio = new MarcaNegocio();
            CategoriaNegocio categoriaNegocio = new CategoriaNegocio();

            try
            {
                btnAceptar.Enabled = false;
                cboMarca.DataSource = marcaNegocio.listar();
                cboMarca.ValueMember = "Id";
                cboMarca.DisplayMember = "Descripcion";
                cboCategoria.DataSource = categoriaNegocio.listar();
                cboCategoria.ValueMember = "Id";
                cboCategoria.DisplayMember = "Descripcion";

                if (articulo != null)
                {
                    txtCodigo.Text = articulo.Codigo;
                    txtNombre.Text = articulo.Nombre;
                    txtDescripcion.Text = articulo.Descripcion;
                    txtUrlImagen.Text = articulo.ImagenUrl;
                    cboMarca.SelectedValue = articulo.Marca.Id;
                    cboCategoria.SelectedValue = articulo.Categoria.Id;
                    txtPrecio.Text = articulo.Precio.ToString(); 
                }

                txtCodigo.TextChanged += HabilitarBtnAceptar;  //Arranco con el btnAceptar deshabilitado
                txtNombre.TextChanged += HabilitarBtnAceptar;  //A medida que voy llenando los casilleros, activo la funcion Habilitar
                txtDescripcion.TextChanged += HabilitarBtnAceptar;
                cboMarca.SelectedIndexChanged += HabilitarBtnAceptar;
                cboCategoria.SelectedIndexChanged += HabilitarBtnAceptar;
                txtPrecio.TextChanged += HabilitarBtnAceptar;

            }
            catch ( Exception ex)
            {

                MessageBox.Show (ex.ToString());
            }
        }
        private void HabilitarBtnAceptar(object sender, EventArgs e)
        {
            btnAceptar.Enabled = !string.IsNullOrEmpty(txtCodigo.Text) &&
                                !string.IsNullOrEmpty(txtNombre.Text) &&         //Se hbilita solo si no estan vacíos estos casilleros
                                !string.IsNullOrEmpty(txtDescripcion.Text) &&
                                cboMarca.SelectedIndex >= 0 &&
                                cboCategoria.SelectedIndex >= 0 &&
                                !string.IsNullOrEmpty(txtPrecio.Text);
        }
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
         
            try
            {
                if (articulo == null)
                    articulo = new Articulo();
               
                articulo.Codigo = txtCodigo.Text;
                articulo.Nombre = txtNombre.Text;
                articulo.Descripcion = txtDescripcion.Text;
                articulo.Marca = (Marca)cboMarca.SelectedItem;
                articulo.Categoria = (Categoria)cboCategoria.SelectedItem;
                articulo.ImagenUrl = txtUrlImagen.Text;
                articulo.Precio = decimal.Parse(txtPrecio.Text);

                if (articulo.Id != 0)
                {
                    negocio.modificar(articulo);
                    MessageBox.Show("Modificado exitosamente");
                }
                else
                {
                    negocio.agregar(articulo);
                    MessageBox.Show("Agregado exitosamente");
                }

                if (archivo != null && !(txtUrlImagen.Text.ToUpper().Contains("HTTP"))) //quiere decir que levanto imagen local, debo hacer una copia
                {
                    string nombreArchivo = Path.GetFileName(archivo.FileName.ToString());
                    string carpetaDestino = "C:\\imagenes-articulos";                     //estas funciones las use porque la referencia configuracion me desaparecia los formularios
                    string nombreDestino = archivo.FileName.ToString() + "nue";
                    string rutaDestino = Path.Combine(carpetaDestino, nombreArchivo);
                    File.Copy(archivo.FileName, rutaDestino ); 
                }

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void cargarImagen(string imagen)
        {
            try
            {
                pbxArticulos.Load(imagen);
            }
            catch (Exception)
            {

                pbxArticulos.Load("https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRKZ1SMO3FUQBP7gzSU3d1Rr1SqSIQzqKdqVA&usqp=CAU");
            }

        }
        private void txtUrlImagen_Leave(object sender, EventArgs e)
        {
            cargarImagen(txtUrlImagen.Text);
        }

        private void btnExaminar_Click(object sender, EventArgs e)
        {
            archivo = new OpenFileDialog(); //archivo ya no esta nulo
            archivo.Filter = "jpg|* .jpg; |png|* .png";

            if (archivo.ShowDialog() == DialogResult.OK)
            {
                txtUrlImagen.Text = archivo.FileName;  //pone el nombre de la ruta local
                cargarImagen(archivo.FileName); //carga imagen local (en btn Aceptar, la guarda)

                
            }

        }
    }
}
