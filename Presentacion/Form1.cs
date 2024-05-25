using Business;
using Dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentacion
{
    public partial class frmCatalogo : Form
    {
        private List<Articulo> listaArticulo;
        public frmCatalogo()
        {
            InitializeComponent();
        }

        private void frmCatalogo_Load(object sender, EventArgs e)
        {
            cargar();
            cboCampo.Items.Add("Codigo");
            cboCampo.Items.Add("Nombre");
            cboCampo.Items.Add("Descripcion");
            cboCampo.Items.Add("Precio");


        }

        
        
        private void cargar()
        {
            ArticuloBusiness negocio = new ArticuloBusiness();
            try
            {
                listaArticulo = negocio.Listar();
                dgvArticulos.DataSource = listaArticulo;
                OcultarColumna();
                CargarImagen(listaArticulo[0].ImagenUrl);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void OcultarColumna()
        {
            dgvArticulos.Columns["ImagenUrl"].Visible = false;
            dgvArticulos.Columns["Id"].Visible = false;
        }


        private void CargarImagen(string imagen)
        {
           try
            {
                if (imagen.ToUpper().Contains("HTTPS") || File.Exists(imagen))
                {
                    
                    pbxArticulo.Load(imagen);
                }
                else
                {
                    pbxArticulo.Load("https://efectocolibri.com/wp-content/uploads/2021/01/placeholder.png");
                    //pbxArticulo.Image = Image.FromFile("C:\\Users\\Administrador\\Desktop\\Maxi Programa Nivel 2 C#\\TPFinalNivel2_Frenquelli\\Imagenes\\placeholder.png");
                }
            }
            catch (Exception ex)
            {

                pbxArticulo.Load("https://efectocolibri.com/wp-content/uploads/2021/01/placeholder.png");
                //pbxArticulo.Image = Image.FromFile("C:\\Users\\Administrador\\Desktop\\Maxi Programa Nivel 2 C#\\TPFinalNivel2_Frenquelli\\Imagenes\\placeholder.png");
            }
        }



        private void dgvArticulos_SelectionChanged(object sender, EventArgs e)
        {
            
                if(dgvArticulos.CurrentRow != null)
                {
                    Articulo seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                    CargarImagen(seleccionado.ImagenUrl);

                }

            
        }

        private void btnNuevoProducto_Click(object sender, EventArgs e)
        {
            frmAltaArticulo nuevo = new frmAltaArticulo();
            nuevo.ShowDialog();
            cargar();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if(dgvArticulos.CurrentRow != null)
            {
                Articulo seleccionado;
                seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;

                if(seleccionado != null)
                {
                    frmAltaArticulo modificado = new frmAltaArticulo(seleccionado);
                    modificado.ShowDialog();
                    cargar();

                }
                else
                {
                    MessageBox.Show("Por favor, selecciona un artículo válido para Editar.");
                }

            }
            else
            {
                MessageBox.Show("Por favor, selecciona un artículo para Editar.");
            }
        }

        private void btnEliminarFisico_Click(object sender, EventArgs e)
        {
            ArticuloBusiness negocio = new ArticuloBusiness();
            Articulo seleccionado;
            try
            {
                DialogResult resultado = MessageBox.Show("El registro se eliminará permanentemente", "Eliminado", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if(resultado == DialogResult.Yes)
                {
                    seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                    negocio.EliminarFisico(seleccionado.Id);
                    cargar();

                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }



        private bool ValidarFiltro()
        {
            if (cboCampo.SelectedIndex < 0)
            {
                MessageBox.Show("Debe seleccionar el campo");
                return true;
            }
            if (cboCriterio.SelectedIndex < 0)
            {
                MessageBox.Show("Debe seleccionar el criterio");
                return true;
            }
            if (cboCampo.SelectedItem.ToString() == "Precio")
            {
                if (txtFiltroAvanzado.Text == String.Empty)
                {
                    MessageBox.Show("Debe ingresar un valor");
                    return true;
                }

                if (!(SoloNumeros(txtFiltroAvanzado.Text)))
                {
                    MessageBox.Show("Debe ingresar valor numerico");
                    return true;

                }
            }

            return false;
        }

        private bool SoloNumeros(string cadena)
        {
            foreach(char caracter in cadena)
            {
                if (!(char.IsNumber(caracter)))
                    return false;
            }
            return true;
        }

        private void btnFiltro_Click(object sender, EventArgs e)
        {
            ArticuloBusiness negocio = new ArticuloBusiness();
            try
            {
                if (ValidarFiltro())
                    return;

                string campo = cboCampo.SelectedItem.ToString();
                string criterio = cboCriterio.SelectedItem.ToString();
                string filtro = txtFiltroAvanzado.Text;
                dgvArticulos.DataSource = negocio.Filtrar(campo, criterio, filtro);

            }
            catch (SqlException ex)
            {
                MessageBox.Show("Hubo un error al intentar filtrar. Por favor, inténtalo de nuevo.");
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error inesperado. Por favor, inténtalo de nuevo.");
                
            }

        }

        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            List<Articulo> listaFiltrada;
            string filtro = txtFiltro.Text;

            if (filtro.Length >= 2)
                listaFiltrada = listaArticulo.FindAll(a => a.Nombre.ToUpper().Contains(filtro.ToUpper()) || a.Marca.Descripcion.ToUpper().Contains(filtro.ToUpper()) || a.Categoria.Descripcion.ToUpper().Contains(filtro.ToUpper()));
            else
                listaFiltrada = listaArticulo;

            dgvArticulos.DataSource = null;
            dgvArticulos.DataSource = listaFiltrada;
            OcultarColumna();
        }

        private void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFiltroAvanzado.Text = "";

            string opcion = cboCampo.SelectedItem.ToString();

            if(opcion == "Precio")
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Menor a");
                cboCriterio.Items.Add("Mayor a");
                cboCriterio.Items.Add("Igual a");
            }
            else
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Comienza con");
                cboCriterio.Items.Add("Termina con");
                cboCriterio.Items.Add("Contiene");
            }
        }

        private void cboCriterio_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFiltroAvanzado.Text = "";
        }

        private void btnDetalle_Click(object sender, EventArgs e)
        {
            if(dgvArticulos.CurrentRow != null)
            {
                Articulo seleccionado;
                seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;

                if(seleccionado != null)
                {
                    frmDetalleArticulo detalle = new frmDetalleArticulo(seleccionado);
                    detalle.ShowDialog();
                    cargar();

                }
                else
                {
                    MessageBox.Show("Por favor, selecciona un artículo válido para Ver el Detalle.");
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un artículo para Ver el Detalle.");
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
