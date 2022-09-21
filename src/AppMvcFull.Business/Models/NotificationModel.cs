namespace AppMvcFull.Business.Models
{
    public class NotificationModel
    {
        public NotificationModel(string mensagem)
        {
            Mensagem = mensagem;
        }

        public string Mensagem { get; }
    }
}
