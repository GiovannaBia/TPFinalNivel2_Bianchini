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

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        
        private List<Articulo> listaArticulo;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {   
            cargar();
            cboCampo.Items.Add("Precio");
            cboCampo.Items.Add("Nombre");
            cboCampo.Items.Add("Marca");
            btnModificar.Enabled = false;
            
        }

        private void dgvArticulos_SelectionChanged(object sender, EventArgs e)
        {
            Articulo seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
            cargarImagen(seleccionado.ImagenUrl);
            btnModificar.Enabled = true;
        }

        private void cargarImagen (string imagen)
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

        private void cargar()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                listaArticulo = negocio.Listar();
                dgvArticulos.DataSource = listaArticulo;
                dgvArticulos.Columns["ImagenUrl"].Visible = false;
                dgvArticulos.Columns["Id"].Visible = false;
                dgvArticulos.RowHeadersVisible = false;
                cargarImagen(listaArticulo[0].ImagenUrl);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmAltaArticulo alta = new frmAltaArticulo();
            alta.ShowDialog();
            cargar();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Articulo seleccionado;
            seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;

            frmAltaArticulo modificar = new frmAltaArticulo(seleccionado); // le hacemos un constructor para pasar esto por parametro
            modificar.ShowDialog();
            cargar();
        }

        private void btnEliminarLogico_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            Articulo seleccionado;
            try
            {
                DialogResult respuesta=  MessageBox.Show("¿Está seguro de eliminar definitivamente este artículo?", "ELIMINANDO", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (respuesta == DialogResult.Yes)
                {
                    seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                    negocio.eliminar(seleccionado.Id);
                    cargar();
                }
            }
            
            catch ( Exception ex)
            {

                MessageBox.Show(ex.ToString()) ;
            }
        }

        private void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion = cboCampo.SelectedItem.ToString();

            if (opcion == "Precio")
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Mayor a ");
                cboCriterio.Items.Add("Menor a ");
                cboCriterio.Items.Add("Igual a ");
            }
            else
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Comienza con ");
                cboCriterio.Items.Add("Termina con ");
                cboCriterio.Items.Add("Contiene ");
            }
        }

        private bool validarFiltro()
        {
            if (cboCampo.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor seleccione un campo");
                return true;     
            }

            if (cboCriterio.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor seleccione un criterio");
                return true;
            }

            if (cboCampo.SelectedItem.ToString() == "Precio")
            {
                if (string.IsNullOrEmpty(txtFiltro.Text))
                {
                    MessageBox.Show("Ingresa un número en el filtro");
                    return true;
                }
               

                if (!(soloNumeros(txtFiltro.Text)))  //Para entrar al Message esta condicion debe dar true, y si no hay solo numeros da false, por eso el !
                {
                    MessageBox.Show("Solo números por favor");
                    return true;
                }

                
            }
            return false;  // Retorna falso si se cumplen los requeridos, es decir,
                           // si hay seleccionados campo y criterio y el campo debidamente llenado
        }

        private bool soloNumeros (string cadena)
        {
            foreach(char caracter in cadena)
            {
                if (!(char.IsNumber(caracter))) // Si no es numero, retorno falso
                return false;       // La logica que seguiamos en el validarFiltro es que si NO se cumple algo
                                    // por esto, acá también lo escribimos así
            }
            return true; //si hay solo numeros, retorna true.
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();

            try
            {
                if (validarFiltro())
                    return;  // Si me develve true, no avanza en el evento, es decir no filtra, para no romper el programa

                string campo = cboCampo.SelectedItem.ToString();
                string criterio = cboCriterio.SelectedItem.ToString();
                string filtro = txtFiltro.Text;
                dgvArticulos.DataSource = negocio.filtrar(campo, criterio, filtro);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void btnLimpiarFiltro_Click(object sender, EventArgs e)
        {
            cargar();
            txtFiltro.Text = "";
           
        }

        private void btnDetalles_Click(object sender, EventArgs e)
        {
            //Convierto en objeto Articulo la fila seleccionada y cargo los parámetros con los atributos del objeto.
            Articulo seleccionada;
            seleccionada = (Articulo) dgvArticulos.CurrentRow.DataBoundItem;

            string codigo = seleccionada.Codigo.ToString();
            string nombre = seleccionada.Nombre.ToString();
            string descripcion = seleccionada.Descripcion.ToString();
            string marca = seleccionada.Marca != null ? seleccionada.Marca.Descripcion.ToString() : string.Empty;
            string categoria = seleccionada.Categoria != null ? seleccionada.Categoria.Descripcion.ToString() : string.Empty;
            string precio = seleccionada.Precio.ToString();
            string imagenUrl = seleccionada.ImagenUrl.ToString();


                frmDetalles detales = new frmDetalles(codigo, nombre, descripcion, marca, categoria, precio, imagenUrl);
                detales.ShowDialog();
            }

              
          
            
            
           
        }
    }

