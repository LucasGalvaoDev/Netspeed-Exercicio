namespace WebApp_Desafio_FrontEnd.ViewModels
{
    public class DataTableAjaxViewModel
    {
        public DataTableAjaxViewModel()
        {
            start = 0;
            length = 10;
        }

        public int start { get; set; }
        public int length { get; set; }
        public object data { get; set; }
    }
}
