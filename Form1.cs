using System.ComponentModel;
using System.Diagnostics;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DGVCambioDeEstadoCheckbox
{
    public partial class Form1 : Form
    {
        //Vamos a crear la lista de elementos del modelo
        private List<Row> jobs;
        public Form1()
        {
            InitializeComponent();
            //Inicializamos el objeto lista de los elementos
            jobs = new List<Row>();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            //Ahora crearemos el formato de columnas q manejara el DGV
            dgvDatos.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id"});
            dgvDatos.Columns.Add(new DataGridViewTextBoxColumn { Name = "Name" });
            dgvDatos.Columns.Add(new DataGridViewTextBoxColumn { Name = "Age" });
            dgvDatos.Columns.Add(new DataGridViewTextBoxColumn { Name = "Email" });
            dgvDatos.Columns.Add(new DataGridViewTextBoxColumn { Name = "Phone" });
            dgvDatos.Columns.Add(new DataGridViewCheckBoxColumn { Name = "Status" });
            //La primera vez debemos revisar si hay elementos y cada q se actualiza un dato de la lista de elementos por lo cual
            //Sera el metodo q recargara el DGV con los datos actualizados
            actualizarvista();
        }
        private void actualizarvista()
        {
            dgvDatos.DataSource = null;
            dgvDatos.Rows.Clear();
            foreach (var item in jobs)
            {
                //generamos un row o fila y le pasamos loa datos por columna
                DataGridViewRow dr = new DataGridViewRow();
                dr.CreateCells(dgvDatos);
                dr.Cells[0].Value = item.id;
                dr.Cells[1].Value = item.name;
                dr.Cells[2].Value = item.age;
                dr.Cells[3].Value = item.email;
                dr.Cells[4].Value = item.phone;
                dr.Cells[5].Value = item.status;
                dgvDatos.Rows.Add(dr);
            }
        }
        //Ahora agregaremos el evento q guarda un elemento en la lista con el boton generado en la vista
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            jobs.Add(
                new Row
                {
                    id = RowId.GetId(),
                    name = textBox1.Text,
                    age = textBox2.Text,
                    email = textBox3.Text,
                    phone = textBox4.Text,
                    status = false
                }
            );
            //falt actualizar la vista despues de cargar un elemento
            actualizarvista();
        }

        private void dgvDatos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //e contendra la referencia de la fila y la columna seleccionada, aparece validar el nombre de la columna se valida
            //si se presiono o se dio click en el contenido de la celda y q haya sido el checkbox y no fuera o alrededor de el
            if (dgvDatos.Columns[e.ColumnIndex].Name.Equals("Status") && dgvDatos.CurrentCell is DataGridViewCheckBoxCell)
            {
                Boolean isChecked = false;
                if (dgvDatos.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null)
                    isChecked = false;
                else
                    isChecked = (Boolean)dgvDatos.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                isChecked = !isChecked;
                //Aqui recuperamos el id modificado en la vista del DGV para actualizarlo en nuestra lista de elementos del modelo
                int id = (int)dgvDatos.Rows[e.RowIndex].Cells["Id"].Value;
                //Aqui recuperamos el elemento del modelo
                var ele = jobs.Where(x => x.id == id).FirstOrDefault();
                //Si encontramos el elemento en nuestro modelo, procedemos a aqctualizarlo
                if (ele != null) ele.status = isChecked;
                dgvDatos.Rows[e.RowIndex].Cells["Status"].Value = isChecked;
            }
        }
        private void btnBorrar_Click(object sender, EventArgs e)
        {
            var rows = dgvDatos.Rows;
            foreach (DataGridViewRow row in rows)
            {
                var idx = row.Index;
                if (row.Selected)
                {
                    var id = (int)row.Cells["Id"].Value;
                    jobs = jobs.Where(x => x.id != id).ToList();
                    //falt actualizar la vista despues de borrar un elemento
                }
            }
            actualizarvista();
        }
        //Ahora solo falta el evento q se encarga de guardar los cambios realizados en el checkbox no en la celda
    }
    //Primero crearemos el modelo a usar en nuestro DGV, usaremos INotifyPropertyChanged para actualizar los datos de los elementos
    public class Row : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private int     _id;
        private String  _name;
        private String  _age;
        private String  _email;
        private String  _phone;
        private Boolean _status;
        public int     id {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                OnPropertyChanged("id");
            }
        }
        public String  name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged("name");
            }
        }
        public String  age {
            get
            {
                return _age;
            }
            set
            {
                _age = value;
                OnPropertyChanged("age");
            }}
        public String  email {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
                OnPropertyChanged("email");
            }}
        public String  phone {
            get
            {
                return _phone;
            }
            set
            {
                _phone = value;
                OnPropertyChanged("phone");
            }}
        public Boolean status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
                OnPropertyChanged("status");
            }
        }
        private void OnPropertyChanged(String propiedad)
        {
            //Cuando la propiedad de uno de los elemento de la lista cambien, se ejecutara este evento q imprimira en consola cuando eso pase
            //Esto es solo para asegurar q la informacion de los elementos del DGV esten identicos al modelo generado
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propiedad));
                Console.WriteLine($"propiedad cambio => {propiedad}, {id}, {name}, {age}, {email}, {phone}, {status}");
                Debug.WriteLine($"propiedad cambio => {propiedad}, {id}, {name}, {age}, {email}, {phone}, {status}");
            }
        }
    }
    //Crearemos una clase estatica para simular los IDS consecutivos de lso elementos
    public static class RowId
    {
        private static int id { get; set; } = 0;
        public static int GetId()
        {
            id++;
            return id;
        }
    }
}