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
using Business;
using System.Configuration;
using System.IO;

namespace Presentacion
{
    public partial class frmAltaArticulo : Form
    {
        private Articulo articulo = null;
        public frmAltaArticulo()
        {
            InitializeComponent();
        }

        public frmAltaArticulo(Articulo articulo)
        {
            InitializeComponent();
            this.articulo = articulo;
            Text = "Editar Articulo";
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private bool ValidarCampos()
        {
            decimal precio;
            if (string.IsNullOrEmpty(txtCodigo.Text) || string.IsNullOrEmpty(txtNombre.Text) || string.IsNullOrEmpty(txtDescripcion.Text) || string.IsNullOrEmpty(txtPrecio.Text) || cboMarca.SelectedItem == null || cboCategoria.SelectedItem == null)
            {
                MessageBox.Show("Complete todos los campos requeridos");
                return true;
            }
            else if(!decimal.TryParse(txtPrecio.Text, out precio))
            {
                MessageBox.Show("El precio debe ser valor numerico");
                return true;
            }
            else
            {
                return false;
            }
        }



        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (ValidarCampos())
                return;


            ArticuloBusiness negocio = new ArticuloBusiness();
            try
            {
                if(articulo == null)
                    articulo = new Articulo();

                articulo.Codigo = txtCodigo.Text;
                articulo.Nombre = txtNombre.Text;
                articulo.Descripcion = txtDescripcion.Text;
                articulo.Precio = decimal.Parse(txtPrecio.Text);
                articulo.ImagenUrl = txtImagen.Text;
                articulo.Marca = (Marca)cboMarca.SelectedItem;
                articulo.Categoria = (Categoria)cboCategoria.SelectedItem;

                if(articulo.Id != 0)
                {
                    negocio.Modificar(articulo);
                    MessageBox.Show("modificado exitosamente");
                }
                else
                {
                    negocio.Agregar(articulo);
                    MessageBox.Show("agregado exitosamente");
                }
                Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void frmAltaArticulo_Load(object sender, EventArgs e)
        {
            MarcaBusiness marcaNegocio = new MarcaBusiness();
            CategoriaBusiness categoriaNegocio = new CategoriaBusiness();
            try
            {
                cboMarca.DataSource = marcaNegocio.Listar();
                cboMarca.ValueMember = "Id";
                cboMarca.DisplayMember = "Descripcion";
                cboCategoria.DataSource = categoriaNegocio.Listar();
                cboCategoria.ValueMember = "Id";
                cboCategoria.DisplayMember = "Descripcion";

                if (articulo != null)
                {
                    txtCodigo.Text = articulo.Codigo;
                    txtNombre.Text = articulo.Nombre;
                    txtDescripcion.Text = articulo.Descripcion;
                    txtPrecio.Text = articulo.Precio.ToString();
                    txtImagen.Text = articulo.ImagenUrl;
                    CargarImagen(articulo.ImagenUrl);
                    cboMarca.SelectedValue = articulo.Marca.Id;
                    cboCategoria.SelectedValue = articulo.Categoria.Id;

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void txtImagen_Leave(object sender, EventArgs e)
        {
            CargarImagen(txtImagen.Text);
        }

        private void CargarImagen(string imagen)
        {
            try
            {
                if (imagen.ToUpper().Contains("HTTPS") || File.Exists(imagen))
                {

                      pbxImagen.Load(imagen);
                }
                else
                {
                    
                    pbxImagen.Load("https://efectocolibri.com/wp-content/uploads/2021/01/placeholder.png");
                    //pbxImagen.Image = Image.FromFile("C:\\Users\\Administrador\\Desktop\\Maxi Programa Nivel 2 C#\\TPFinalNivel2_Frenquelli\\Imagenes\\placeholder.png");
                }
            }
            catch (Exception ex)
            {
                pbxImagen.Load("https://efectocolibri.com/wp-content/uploads/2021/01/placeholder.png");
                //pbxImagen.Image = Image.FromFile("C:\\Users\\Administrador\\Desktop\\Maxi Programa Nivel 2 C#\\TPFinalNivel2_Frenquelli\\Imagenes\\placeholder.png");
            }
        }

        private void btnBuscarImagen_Click(object sender, EventArgs e)
        {
             OpenFileDialog archivo = new OpenFileDialog();
             archivo.Filter = "jpg|*.jpg;|png|*.png";
            //if (archivo.ShowDialog() == DialogResult.OK)
            //{
            //    txtImagen.Text = archivo.FileName;
            //    CargarImagen(archivo.FileName);

            //    File.Copy(archivo.FileName, ConfigurationManager.AppSettings["articulos-app"] + archivo.SafeFileName);
            //}

                if (archivo.ShowDialog() == DialogResult.OK)
                {
                    string rutaImagen = archivo.FileName;
                    txtImagen.Text = rutaImagen;

                    string destino = ConfigurationManager.AppSettings["articulos-app"] + archivo.SafeFileName;
                    if (File.Exists(destino))
                    {
                        DialogResult resultado = MessageBox.Show("La imagen ya existe. ¿Desea reutilizarla?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (resultado == DialogResult.Yes)
                        {
                            File.Copy(rutaImagen, destino, true);
                            CargarImagen(rutaImagen);
                        }
                        else
                        {
                            txtImagen.Text = String.Empty;
                        }
                    }
                    else
                    {
                        File.Copy(rutaImagen, destino);
                        CargarImagen(rutaImagen);
                    }

                    
                }
        }

    }
}
