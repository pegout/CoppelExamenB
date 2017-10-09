using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using examen.BLL;

namespace examen
{
    public partial class Usuarios : Form
    {
        bool editando = false;
        int usuarioid;

        // ----------------------- METODOS ----------------------- //
        public Usuarios()
        {
            InitializeComponent();
        }

        private void limpiarTextos()
        {
            txtClave.Text = String.Empty;
            txtNombre.Text = String.Empty;
            txtApellido.Text = String.Empty;
            dtpFechaNacimiento.Text = DateTime.Today.ToString();
            editando = false;
            txtClave.Focus();
        }

        private bool validaDatos()
        {
            if (txtClave.Text.Length < 3 || txtNombre.Text.Length < 3 || txtApellido.Text.Length < 3)
            {
                MessageBox.Show("Por favor, verifique que todos los campos tengan al menos 3 caracteres.", "Atención!");
                return false;
            }
            return true;
        }

        private void cargarGrid()
        {
            UsuarioBLL obj = new UsuarioBLL();
            this.usuarioDataGridView.DataSource = obj.obtenerUsuario();
            usuarioDataGridView.Columns["UsuarioID"].Width = 60;
            usuarioDataGridView.Columns["Clave"].Width = 40;
            usuarioDataGridView.Columns["Nombre"].Width = 150;
            usuarioDataGridView.Columns["Apellido"].Width = 150;
            usuarioDataGridView.Columns["FechaNacimiento"].Width = 80;
            usuarioDataGridView.Columns["FechaNacimiento"].HeaderText = "Fecha Nacimiento";
        }

        // ----------------------- EVENTOS ----------------------- //
        private void Usuarios_Load(object sender, EventArgs e)
        {
            try
            {
                cargarGrid();
            }
            catch
            {
                MessageBox.Show("Ocurrió un error al obtener los usuarios.","Error!");
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            UsuarioBLL usuario = new UsuarioBLL();
            usuario.Clave = txtClave.Text;
            usuario.Nombre = txtNombre.Text;
            usuario.Apellido = txtApellido.Text;
            usuario.FechaNacimiento = dtpFechaNacimiento.Text;
            usuario.UsuarioID = usuarioid;

            try
            {
                if (validaDatos())
                {
                    if (editando)
                        usuario.actualizarUsuario(usuario);
                    else
                        usuario.guardarUsuario(usuario);

                    limpiarTextos();
                }

                cargarGrid();
            }
            catch
            {
                MessageBox.Show("Ocurrió un error al intentar guardar.", "Error!");
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            UsuarioBLL usuario = new UsuarioBLL();
            DialogResult eliminar;

            try
            {
                if (editando)
                {
                    eliminar = MessageBox.Show("¿Está seguro que desea eliminar este registro?", "Eliminar Usuario", MessageBoxButtons.YesNo);
                    if (eliminar == DialogResult.Yes)
                    {
                        usuario.eliminarUsuarioFisico(usuarioid);
                        limpiarTextos();
                        cargarGrid();
                    }
                }
                else
                    MessageBox.Show("Por favor, seleccione un registro a eliminar.", "Atención!");
            }
            catch
            {
                MessageBox.Show("Ocurrió un error al intentar eliminar.", "Error!");
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            limpiarTextos();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void usuarioDataGridView_MouseClick(object sender, MouseEventArgs e)
        {
            if (usuarioDataGridView.RowCount > 0)
            {
                usuarioid = Convert.ToInt32(usuarioDataGridView.SelectedRows[0].Cells[0].Value.ToString());
                txtClave.Text = usuarioDataGridView.SelectedRows[0].Cells[1].Value.ToString();
                txtNombre.Text = usuarioDataGridView.SelectedRows[0].Cells[2].Value.ToString();
                txtApellido.Text = usuarioDataGridView.SelectedRows[0].Cells[3].Value.ToString();
                dtpFechaNacimiento.Text = usuarioDataGridView.SelectedRows[0].Cells[4].Value.ToString();
                editando = true;
            }
            else
                txtClave.Focus();
        }
    }
}
