using Dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentacion
{
    public partial class frmDetalleArticulo : Form
    {
        private Articulo articulo;

        public frmDetalleArticulo()
        {
            InitializeComponent();
        }

        public frmDetalleArticulo(Articulo articulo)
        {
            InitializeComponent();
            this.articulo = articulo;
        }

        private void CargarImagen(string imagen)
        {
            try
            {
                if (imagen.ToUpper().Contains("HTTPS") || File.Exists(imagen))
                {

                    pbxImagenDetalle.Load(imagen);
                }
                else
                {

                    pbxImagenDetalle.Load("https://efectocolibri.com/wp-content/uploads/2021/01/placeholder.png");
                    //pbxImagen.Image = Image.FromFile("C:\\Users\\Administrador\\Desktop\\Maxi Programa Nivel 2 C#\\TPFinalNivel2_Frenquelli\\Imagenes\\placeholder.png");
                }
            }
            catch (Exception ex)
            {
                pbxImagenDetalle.Load("https://efectocolibri.com/wp-content/uploads/2021/01/placeholder.png");
                //pbxImagen.Image = Image.FromFile("C:\\Users\\Administrador\\Desktop\\Maxi Programa Nivel 2 C#\\TPFinalNivel2_Frenquelli\\Imagenes\\placeholder.png");
            }
        }

        private void frmDetalleArticulo_Load(object sender, EventArgs e)
        {
            try
            {
                lblCategoriaTexDetalle.Text = articulo.Categoria.ToString();
                lblMarcaTextDetalle.Text = articulo.Marca.ToString();
                lblNombreTextDetalle.Text = articulo.Nombre.ToString();
                lblCodigoTextDetalle.Text = articulo.Codigo.ToString();
                lblPrecioTextDetalle.Text = articulo.Precio.ToString();
                lblDescripcionTextDetalle.Text = articulo.Descripcion.ToString();
                CargarImagen(articulo.ImagenUrl);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void btnCerrarDetalle_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
