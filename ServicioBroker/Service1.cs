using System.ServiceProcess;

namespace ServicioBroker
{
    public partial class Service1 : ServiceBase
    {
       
        public Service1()
        {
            ServiceName = ServicioInicial.NombreServicio;
            //InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            ServicioInicial.RunAsConsole(null);
        }

        protected override void OnStop()
        {
            ServicioInicial.Stop();
        }


        //private void busqueda_maquinas(DataTable table)
        //{
        //    foreach (System.Data.DataRow row in table.Rows)
        //    {
        //        foreach (System.Data.DataColumn col in table.Columns)
        //        {
        //            Console.WriteLine("{0} = {1}", col.ColumnName, row[col]);
        //        }
        //        Console.WriteLine("============================");
        //    }
        //}

    }
}
