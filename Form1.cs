using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MySql.Data;
using MySql.Data.MySqlClient;

namespace Semana7_Proyecto_Login
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnAccess_Click(object sender, EventArgs e)
        {
            try
            {
                //variables globales
                Variables cadenas = new Variables();
                //Creado la variable para la nueva conexion
                OleDbConnection conexion_access = new OleDbConnection();
                //Cadena de conexión para la base de datos
                //Se recomienda generar la cadena de conexion para evitar errores
                //conexion_access.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\sistema.accdb;Persist Security Info=False;";
                conexion_access.ConnectionString = @cadenas.conexionAcces;
                //Abriendo conexion
                conexion_access.Open();
                //Consulta a tabla de usuarios en la base de datos
                //Para encontrar fila que tiene los datos del usuario y clave ingresados
                OleDbDataAdapter consulta = new OleDbDataAdapter("SELECT * FROM usuarios", conexion_access);
                //OleDbDataReader reader = command.ExecuteReader();
                DataSet resultado = new DataSet();
                consulta.Fill(resultado);
                foreach (DataRow registro in resultado.Tables[0].Rows)
                {
                    if ((txtUsuario.Text == registro["usuario"].ToString()) && (txtPassword.Text == registro["password"].ToString()))
                    {
                        //llamando formulario principal llamado menu
                        FormPrincipal fp = new FormPrincipal();
                        fp.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Error de usuario o clave de acceso", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                } //Cierre de ciclo for
                conexion_access.Close();
            } //Cierre de Try
              //Si la conexion falla muestra mensaje de error
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
                //en caso que usuario y clave sean incorrectos mostrar mensaje de error
                MessageBox.Show("Error de usuario o clave de acceso", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtUsuario.Focus();
            }
            //Finalizando la conexión
        }

        private void btnSQL_Click(object sender, EventArgs e)
        {
            try
            {
                //variables globales
                Variables cadenas = new Variables();

                //crear la conexion
                SqlConnection conexion = new SqlConnection(@cadenas.conexionSQL);
                //abrir conexion
                conexion.Open();

                //cadena de consulta
                string consultax;
                consultax = "select usuario, password from usuarios where usuario = '" + txtUsuario.Text + "'And password = '" + txtPassword.Text + "' ";
                SqlCommand consulta = new SqlCommand(consultax, conexion);
                //ejecuta una instruccion de sql devolviendo el numero de filas encontradas
                consulta.ExecuteNonQuery();
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(consulta);
                //Llenando el dataAdapter con los datos de la tabla
                da.Fill(ds, "usuarios");
                //fila de la tabla con la que se trabajara
                DataRow registro;
                registro = ds.Tables["usuarios"].Rows[0];
                //evaluando que clave y usuario sean correctos
                if ((txtUsuario.Text == registro["usuario"].ToString()) || (txtPassword.Text == registro["password"].ToString()))
                {
                    //llamando formulario principal llamado menu
                    FormPrincipal fp = new FormPrincipal();
                    fp.Show();
                    this.Hide();
                }
            }
            catch
            {
                //en caso que la clave sea incorrecta mostrar mensaje de error
                MessageBox.Show("Error de usuario o clave de acceso", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnMySQL_Click(object sender, EventArgs e)
        {
            //variables globales
            Variables cadenas = new Variables();
            string connStr = cadenas.conexionMySql;
            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();

                string sql = "SELECT usuario, status FROM usuarios WHERE usuario='" + txtUsuario.Text.Trim() + "' and password='" + txtPassword.Text.Trim() + "'";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                int contador = 0;
                while (rdr.Read())
                {
                    contador++;
                }
                rdr.Close();

                if (contador > 0)
                {
                    FormPrincipal fp = new FormPrincipal();
                    fp.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Error de usuario o clave de acceso", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            conn.Close();
        }
    }
}
